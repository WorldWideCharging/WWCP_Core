﻿/*
 * Copyright (c) 2014-2019 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace org.GraphDefined.WWCP.Virtual
{

    /// <summary>
    /// A virtual EVSE.
    /// </summary>
    public class VirtualEVSE : AEMobilityEntity<EVSE_Id>,
                               IEquatable<VirtualEVSE>, IComparable<VirtualEVSE>, IComparable,
                               IEnumerable<SocketOutlet>,
                               IStatus<EVSEStatusTypes>,
                               IRemoteEVSE
    {

        #region Data

        /// <summary>
        /// The default max size of the status history.
        /// </summary>
        public const UInt16 DefaultMaxStatusListSize        = 50;

        /// <summary>
        /// The default max size of the admin status history.
        /// </summary>
        public const UInt16 DefaultMaxAdminStatusListSize   = 50;

        /// <summary>
        /// The maximum time span for a reservation.
        /// </summary>
        public  static readonly TimeSpan  MaxReservationDuration    = TimeSpan.FromMinutes(15);

        /// <summary>
        /// The default time span between self checks.
        /// </summary>
        public  static readonly TimeSpan  DefaultSelfCheckTimeSpan  = TimeSpan.FromSeconds(15);

        private static readonly Random    _random                   = new Random(DateTime.UtcNow.Millisecond);

        private        readonly Object    EnergyMeterLock;
        private                 Timer     EnergyMeterTimer;

        private        readonly Object    ReservationExpiredLock;
        private        readonly Timer     ReservationExpiredTimer;

        #endregion

        #region Properties

        /// <summary>
        /// The identification of the operator of this virtual EVSE.
        /// </summary>
        [InternalUseOnly]
        public ChargingStationOperator_Id OperatorId
            => Id.OperatorId;

        #region Description

        private I18NString _Description;

        /// <summary>
        /// An description of this EVSE.
        /// </summary>
        [Mandatory]
        public I18NString Description
        {

            get
            {
                return _Description;
            }

            set
            {
                if (_Description != value)
                    SetProperty(ref _Description, value);
            }

        }

        #endregion


        #region ChargingModes

        private ReactiveSet<ChargingModes> _ChargingModes;

        /// <summary>
        /// Charging modes.
        /// </summary>
        [Mandatory]
        public ReactiveSet<ChargingModes> ChargingModes
        {

            get
            {
                return _ChargingModes;
            }

            set
            {

                if (_ChargingModes != value)
                    SetProperty(ref _ChargingModes, value);

            }

        }

        #endregion

        #region AverageVoltage

        private Double _AverageVoltage;

        /// <summary>
        /// The average voltage.
        /// </summary>
        [Mandatory]
        public Double AverageVoltage
        {

            get
            {
                return _AverageVoltage;
            }

            set
            {

                if (_AverageVoltage != value)
                    SetProperty(ref _AverageVoltage, value);

            }

        }

        #endregion

        #region CurrentType

        private CurrentTypes _CurrentType;

        /// <summary>
        /// The type of the current.
        /// </summary>
        [Mandatory]
        public CurrentTypes CurrentType
        {

            get
            {
                return _CurrentType;
            }

            set
            {

                if (_CurrentType != value)
                    SetProperty(ref _CurrentType, value);

            }

        }

        #endregion

        #region MaxCurrent

        private Double _MaxCurrent;

        /// <summary>
        /// The maximum current [Ampere].
        /// </summary>
        [Mandatory]
        public Double MaxCurrent
        {

            get
            {
                return _MaxCurrent;
            }

            set
            {

                if (_MaxCurrent != value)
                    SetProperty(ref _MaxCurrent, value);

            }

        }

        #endregion

        #region MaxPower

        private Double _MaxPower;

        /// <summary>
        /// The maximum power [kWatt].
        /// </summary>
        [Mandatory]
        public Double MaxPower
        {

            get
            {
                return _MaxPower;
            }

            set
            {

                if (_MaxPower != value)
                    SetProperty(ref _MaxPower, value);

            }

        }

        #endregion

        #region RealTimePower

        private Double _RealTimePower;

        /// <summary>
        /// The current real-time power delivery [Watt].
        /// </summary>
        [Mandatory]
        public Double RealTimePower
        {

            get
            {
                return _RealTimePower;
            }

            set
            {

                if (_RealTimePower != value)
                    SetProperty(ref _RealTimePower, value);

            }

        }

        #endregion

        #region MaxCapacity

        private Double? _MaxCapacity;

        /// <summary>
        /// The maximum capacity [kWh].
        /// </summary>
        [Mandatory]
        public Double? MaxCapacity
        {

            get
            {
                return _MaxCapacity;
            }

            set
            {

                if (_MaxCapacity != value)
                    SetProperty(ref _MaxCapacity, value);

            }

        }

        #endregion


        #region SocketOutlets

        private ReactiveSet<SocketOutlet> _SocketOutlets;

        public ReactiveSet<SocketOutlet> SocketOutlets
        {

            get
            {
                return _SocketOutlets;
            }

            set
            {

                if (_SocketOutlets != value)
                    SetProperty(ref _SocketOutlets, value);

            }

        }

        #endregion


        #region Status

        /// <summary>
        /// The current EVSE status.
        /// </summary>
        [InternalUseOnly]
        public Timestamped<EVSEStatusTypes> Status
        {

            get
            {

                if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                    AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
                {
                    return _StatusSchedule.CurrentStatus;
                }

                else
                {

                    switch (AdminStatus.Value)
                    {

                        default:
                            return new Timestamped<EVSEStatusTypes>(AdminStatus.Timestamp, EVSEStatusTypes.OutOfService);

                    }

                }

            }

            set
            {

                if (value == null)
                    return;

                if (_StatusSchedule.CurrentValue != value.Value)
                    SetStatus(value);

            }

        }

        #endregion

        #region StatusSchedule

        private StatusSchedule<EVSEStatusTypes> _StatusSchedule;

        /// <summary>
        /// The EVSE status schedule.
        /// </summary>
        public IEnumerable<Timestamped<EVSEStatusTypes>> StatusSchedule
        {
            get
            {
                return _StatusSchedule;
            }
        }

        #endregion

        #region AdminStatus

        /// <summary>
        /// The current EVSE admin status.
        /// </summary>
        [InternalUseOnly]
        public Timestamped<EVSEAdminStatusTypes> AdminStatus
        {

            get
            {
                return _AdminStatusSchedule.CurrentStatus;
            }

            set
            {

                if (value == null)
                    return;

                if (_AdminStatusSchedule.CurrentValue != value.Value)
                    SetAdminStatus(value);

            }

        }

        #endregion

        #region AdminStatusSchedule

        private StatusSchedule<EVSEAdminStatusTypes> _AdminStatusSchedule;

        /// <summary>
        /// The EVSE admin status schedule.
        /// </summary>
        public IEnumerable<Timestamped<EVSEAdminStatusTypes>> AdminStatusSchedule
        {
            get
            {
                return _AdminStatusSchedule;
            }
        }

        #endregion


        /// <summary>
        /// The time span between self checks.
        /// </summary>
        public TimeSpan SelfCheckTimeSpan { get; }


        #region EnergyMeterId

        private EnergyMeter_Id? _EnergyMeterId;

        /// <summary>
        /// The energy meter identification.
        /// </summary>
        [Optional]
        public EnergyMeter_Id? EnergyMeterId
        {

            get
            {
                return _EnergyMeterId;
            }

            set
            {

                if (value != null)
                    SetProperty(ref _EnergyMeterId, value);

                else
                    DeleteProperty(ref _EnergyMeterId);

            }

        }

        #endregion

        public TimeSpan                EnergyMeterInterval      { get; }

        public String                  EllipticCurve            { get; }
        public X9ECParameters          ECP                      { get; }
        public ECDomainParameters      ECSpec                   { get; }
        public FpCurve                 C                        { get; }
        public ECPrivateKeyParameters  PrivateKey               { get; }
        public PublicKeyCertificates   PublicKeyCertificates    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new virtual EVSE.
        /// </summary>
        /// <param name="Id">The unique identification of this EVSE.</param>
        /// <param name="MaxAdminStatusListSize">The maximum size of the EVSE admin status list.</param>
        /// <param name="MaxStatusListSize">The maximum size of the EVSE status list.</param>
        internal VirtualEVSE(EVSE_Id                 Id,
                             I18NString              Description              = null,
                             EVSEAdminStatusTypes    InitialAdminStatus       = EVSEAdminStatusTypes.Operational,
                             EVSEStatusTypes         InitialStatus            = EVSEStatusTypes.Available,
                             EnergyMeter_Id?         EnergyMeterId            = null,
                             String                  EllipticCurve            = "P-256",
                             ECPrivateKeyParameters  PrivateKey               = null,
                             PublicKeyCertificates   PublicKeyCertificates    = null,
                             TimeSpan?               SelfCheckTimeSpan        = null,
                             UInt16                  MaxAdminStatusListSize   = DefaultMaxAdminStatusListSize,
                             UInt16                  MaxStatusListSize        = DefaultMaxStatusListSize)

            : base(Id)

        {

            #region Init data and properties

            this._Description           = Description ?? I18NString.Empty;
            this._ChargingModes         = new ReactiveSet<ChargingModes>();
            this._SocketOutlets         = new ReactiveSet<SocketOutlet>();

            this.EnergyMeterId          = EnergyMeterId;

            this.SelfCheckTimeSpan      = SelfCheckTimeSpan != null && SelfCheckTimeSpan.HasValue ? SelfCheckTimeSpan.Value : DefaultSelfCheckTimeSpan;

            this._AdminStatusSchedule   = new StatusSchedule<EVSEAdminStatusTypes>(MaxAdminStatusListSize);
            this._AdminStatusSchedule.Insert(InitialAdminStatus);

            this._StatusSchedule        = new StatusSchedule<EVSEStatusTypes>(MaxStatusListSize);
            this._StatusSchedule.     Insert(InitialStatus);

            this._Reservations          = new Dictionary<ChargingReservation_Id, ChargingReservation>();

            #endregion

            #region Setup crypto

            this.EllipticCurve          = EllipticCurve ?? "P-256";
            this.ECP                    = ECNamedCurveTable.GetByName(this.EllipticCurve);
            this.ECSpec                 = new ECDomainParameters(ECP.Curve, ECP.G, ECP.N, ECP.H, ECP.GetSeed());
            this.C                      = (FpCurve) ECSpec.Curve;
            this.PrivateKey             = PrivateKey;
            this.PublicKeyCertificates  = PublicKeyCertificates;

            if (PrivateKey == null && PublicKeyCertificates == null)
            {

                var generator = GeneratorUtilities.GetKeyPairGenerator("ECDH");
                generator.Init(new ECKeyGenerationParameters(ECSpec, new SecureRandom()));

                var  keyPair                = generator.GenerateKeyPair();
                this.PrivateKey             = keyPair.Private as ECPrivateKeyParameters;
                this.PublicKeyCertificates  = new PublicKeyCertificate(
                                                  PublicKeys:      new PublicKeyLifetime[] {
                                                                       new PublicKeyLifetime(
                                                                           PublicKey:  keyPair.Public as ECPublicKeyParameters,
                                                                           NotBefore:  DateTime.UtcNow,
                                                                           NotAfter:   DateTime.UtcNow + TimeSpan.FromDays(365),
                                                                           Algorithm:  "P-256",
                                                                           Comment:    I18NString.Empty
                                                                       )
                                                                   },
                                                  Description:     I18NString.Create(Languages.eng, "Auto-generated test keys for a virtual EVSE!"),
                                                  Operations:      JSONObject.Create(
                                                                       new JProperty("signMeterValues",  true),
                                                                       new JProperty("signCertificates", false)
                                                                   ),
                                                  EVSEId:          Id,
                                                  EnergyMeterId:   EnergyMeterId);

            }

            #endregion

            #region Link events

            this._AdminStatusSchedule.OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

            this._StatusSchedule.     OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

            #endregion

            ReservationExpiredLock   = new Object();
            ReservationExpiredTimer  = new Timer(CheckIfReservationIsExpired, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));

            EnergyMeterLock          = new Object();
            EnergyMeterTimer         = new Timer(ReadEnergyMeter,             null, Timeout.Infinite,        Timeout.Infinite);
            EnergyMeterInterval      = TimeSpan.FromSeconds(30);

        }

        #endregion


        #region (private, Timer) ReadEnergyMeter(Status)

        private void ReadEnergyMeter(Object Status)
        {

            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            if (Monitor.TryEnter(EnergyMeterLock))
            {

                try
                {

                    ChargingSession.AddEnergyMeterValue(new Timestamped<Single>(DateTime.UtcNow, 1));

                }
                catch (Exception e)
                {
                    DebugX.LogT("'ReadEnergyMeter' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);
                }

                finally
                {
                    Monitor.Exit(EnergyMeterLock);
                }

            }

            else
                DebugX.LogT("'ReadEnergyMeter' skipped!");

        }

        #endregion



        #region Data/(Admin-)Status management

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of the EVSE changed.
        /// </summary>
        public event OnRemoteEVSEDataChangedDelegate         OnDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of the EVSE changed.
        /// </summary>
        public event OnRemoteEVSEStatusChangedDelegate       OnStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status of the EVSE changed.
        /// </summary>
        public event OnRemoteEVSEAdminStatusChangedDelegate  OnAdminStatusChanged;

        #endregion


        #region SetStatus(NewStatus)

        /// <summary>
        /// Set the current status.
        /// </summary>
        /// <param name="NewStatus">A new status.</param>
        public void SetStatus(EVSEStatusTypes  NewStatus)
        {
            _StatusSchedule.Insert(NewStatus);
        }

        #endregion

        #region SetStatus(NewTimestampedStatus)

        /// <summary>
        /// Set the current status.
        /// </summary>
        /// <param name="NewTimestampedStatus">A new timestamped status.</param>
        public void SetStatus(Timestamped<EVSEStatusTypes> NewTimestampedStatus)
        {
            _StatusSchedule.Insert(NewTimestampedStatus);
        }

        #endregion

        #region SetStatus(NewStatus, Timestamp)

        /// <summary>
        /// Set the status.
        /// </summary>
        /// <param name="NewStatus">A new status.</param>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        public void SetStatus(EVSEStatusTypes  NewStatus,
                              DateTime        Timestamp)
        {
            _StatusSchedule.Insert(NewStatus, Timestamp);
        }

        #endregion

        #region SetStatus(NewStatusList, ChangeMethod = ChangeMethods.Replace)

        /// <summary>
        /// Set the timestamped status.
        /// </summary>
        /// <param name="NewStatusList">A list of new timestamped status.</param>
        /// <param name="ChangeMethod">The change mode.</param>
        public void SetStatus(IEnumerable<Timestamped<EVSEStatusTypes>>  NewStatusList,
                              ChangeMethods                             ChangeMethod = ChangeMethods.Replace)
        {
            _StatusSchedule.Insert(NewStatusList, ChangeMethod);
        }

        #endregion


        #region SetAdminStatus(NewAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(EVSEAdminStatusTypes NewAdminStatus)
        {
            _AdminStatusSchedule.Insert(NewAdminStatus);
        }

        #endregion

        #region SetAdminStatus(NewTimestampedAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewTimestampedAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(Timestamped<EVSEAdminStatusTypes> NewTimestampedAdminStatus)
        {
            _AdminStatusSchedule.Insert(NewTimestampedAdminStatus);
        }

        #endregion

        #region SetAdminStatus(NewAdminStatus, Timestamp)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewAdminStatus">A new admin status.</param>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        public void SetAdminStatus(EVSEAdminStatusTypes  NewAdminStatus,
                                   DateTime             Timestamp)
        {
            _AdminStatusSchedule.Insert(NewAdminStatus, Timestamp);
        }

        #endregion

        #region SetAdminStatus(NewAdminStatusList, ChangeMethod = ChangeMethods.Replace)

        /// <summary>
        /// Set the timestamped admin status.
        /// </summary>
        /// <param name="NewAdminStatusList">A list of new timestamped admin status.</param>
        /// <param name="ChangeMethod">The change mode.</param>
        public void SetAdminStatus(IEnumerable<Timestamped<EVSEAdminStatusTypes>>  NewAdminStatusList,
                                   ChangeMethods                                  ChangeMethod = ChangeMethods.Replace)
        {
            _AdminStatusSchedule.Insert(NewAdminStatusList, ChangeMethod);
        }

        #endregion


        #region (internal) UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="OldStatus">The old EVSE admin status.</param>
        /// <param name="NewStatus">The new EVSE admin status.</param>
        internal async Task UpdateAdminStatus(DateTime                           Timestamp,
                                              EventTracking_Id                   EventTrackingId,
                                              Timestamped<EVSEAdminStatusTypes>  OldStatus,
                                              Timestamped<EVSEAdminStatusTypes>  NewStatus)
        {

            var OnAdminStatusChangedLocal = OnAdminStatusChanged;
            if (OnAdminStatusChangedLocal != null)
                await OnAdminStatusChangedLocal(Timestamp,
                                                EventTrackingId,
                                                this,
                                                OldStatus,
                                                NewStatus);

        }

        #endregion

        #region (internal) UpdateStatus     (Timestamp, EventTrackingId, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateStatus(DateTime                      Timestamp,
                                         EventTracking_Id              EventTrackingId,
                                         Timestamped<EVSEStatusTypes>  OldStatus,
                                         Timestamped<EVSEStatusTypes>  NewStatus)
        {

            var OnStatusChangedLocal = OnStatusChanged;
            if (OnStatusChangedLocal != null)
                await OnStatusChangedLocal(Timestamp,
                                           EventTrackingId,
                                           this,
                                           OldStatus,
                                           NewStatus);

        }

        #endregion

        #endregion

        #region Reservations...

        #region Data

        private readonly Dictionary<ChargingReservation_Id, ChargingReservation> _Reservations;

        /// <summary>
        /// All current charging reservations.
        /// </summary>
        public IEnumerable<ChargingReservation> Reservations
            => _Reservations.Select(_ => _.Value);

        #region TryGetReservationById(ReservationId, out Reservation)

        /// <summary>
        /// Return the charging reservation specified by the given identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        /// <param name="Reservation">The charging reservation.</param>
        public Boolean TryGetChargingReservationById(ChargingReservation_Id ReservationId, out ChargingReservation Reservation)
            => _Reservations.TryGetValue(ReservationId, out Reservation);

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a charging location is being reserved.
        /// </summary>
        public event OnReserveRequestDelegate             OnReserveRequest;

        /// <summary>
        /// An event fired whenever a charging location was reserved.
        /// </summary>
        public event OnReserveResponseDelegate            OnReserveResponse;

        /// <summary>
        /// An event fired whenever a new charging reservation was created.
        /// </summary>
        public event OnNewReservationDelegate             OnNewReservation;


        /// <summary>
        /// An event fired whenever a charging reservation is being canceled.
        /// </summary>
        public event OnCancelReservationRequestDelegate   OnCancelReservationRequest;

        /// <summary>
        /// An event fired whenever a charging reservation was canceled.
        /// </summary>
        public event OnCancelReservationResponseDelegate  OnCancelReservationResponse;

        /// <summary>
        /// An event fired whenever a charging reservation was canceled.
        /// </summary>
        public event OnReservationCanceledDelegate        OnReservationCanceled;

        #endregion

        #region Reserve(                                           StartTime = null, Duration = null, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at this EVSE.
        /// </summary>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="RemoteAuthentication">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
        /// <param name="ChargingProduct">The charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<ReservationResult>

            Reserve(DateTime?                         StartTime              = null,
                    TimeSpan?                         Duration               = null,
                    ChargingReservation_Id?           ReservationId          = null,
                    eMobilityProvider_Id?             ProviderId             = null,
                    RemoteAuthentication              RemoteAuthentication   = null,
                    ChargingProduct                   ChargingProduct        = null,
                    IEnumerable<Auth_Token>           AuthTokens             = null,
                    IEnumerable<eMobilityAccount_Id>  eMAIds                 = null,
                    IEnumerable<UInt32>               PINs                   = null,

                    DateTime?                         Timestamp              = null,
                    CancellationToken?                CancellationToken      = null,
                    EventTracking_Id                  EventTrackingId        = null,
                    TimeSpan?                         RequestTimeout         = null)


                => Reserve(ChargingLocation.FromEVSEId(Id),
                           ChargingReservationLevel.EVSE,
                           StartTime,
                           Duration,
                           ReservationId,
                           ProviderId,
                           RemoteAuthentication,
                           ChargingProduct,
                           AuthTokens,
                           eMAIds,
                           PINs,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout);

        #endregion

        #region Reserve(ChargingLocation, ReservationLevel = EVSE, StartTime = null, Duration = null, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at the given charging location.
        /// </summary>
        /// <param name="ChargingLocation">A charging location.</param>
        /// <param name="ReservationLevel">The level of the reservation to create (EVSE, charging station, ...).</param>
        /// <param name="ReservationStartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="RemoteAuthentication">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
        /// <param name="ChargingProduct">The charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<ReservationResult>

            Reserve(ChargingLocation                  ChargingLocation,
                    ChargingReservationLevel          ReservationLevel       = ChargingReservationLevel.EVSE,
                    DateTime?                         ReservationStartTime   = null,
                    TimeSpan?                         Duration               = null,
                    ChargingReservation_Id?           ReservationId          = null,
                    eMobilityProvider_Id?             ProviderId             = null,
                    RemoteAuthentication              RemoteAuthentication   = null,
                    ChargingProduct                   ChargingProduct        = null,
                    IEnumerable<Auth_Token>           AuthTokens             = null,
                    IEnumerable<eMobilityAccount_Id>  eMAIds                 = null,
                    IEnumerable<UInt32>               PINs                   = null,

                    DateTime?                         Timestamp              = null,
                    CancellationToken?                CancellationToken      = null,
                    EventTracking_Id                  EventTrackingId        = null,
                    TimeSpan?                         RequestTimeout         = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            ChargingReservation newReservation  = null;
            ReservationResult   result          = null;

            #endregion

            #region Send OnReserveRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnReserveRequest?.Invoke(StartTime,
                                         Timestamp.Value,
                                         this,
                                         EventTrackingId,
                                         ReservationId,
                                         ChargingLocation,
                                         ReservationStartTime,
                                         Duration,
                                         ProviderId,
                                         RemoteAuthentication,
                                         ChargingProduct,
                                         AuthTokens,
                                         eMAIds,
                                         PINs,
                                         RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(VirtualEVSE) + "." + nameof(OnReserveRequest));
            }

            #endregion

            try
            {

                if (ChargingLocation.EVSEId.HasValue && ChargingLocation.EVSEId.Value != Id)
                    result = ReservationResult.UnknownLocation;

                else if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                         AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
                {

                    lock (_Reservations)
                    {

                        #region Check if this is a reservation update...

                        if (ReservationId.HasValue &&
                            _Reservations.TryGetValue(ReservationId.Value, out ChargingReservation oldReservation))
                        {

                            //ToDo: Calc if this reservation update is possible!
                            //      When their are other reservations => conflicts!

                            var updatedReservation  = _Reservations[ReservationId.Value]
                                                    = new ChargingReservation(oldReservation.Id,
                                                                              Timestamp.Value,
                                                                              oldReservation.StartTime,
                                                                              Duration ?? MaxReservationDuration,
                                                                              (ReservationStartTime ?? DateTime.UtcNow) + (Duration ?? MaxReservationDuration),
                                                                              oldReservation.ConsumedReservationTime + oldReservation.Duration - oldReservation.TimeLeft,
                                                                              ReservationLevel,
                                                                              ProviderId,
                                                                              RemoteAuthentication,
                                                                              null, //ChargingStation.ChargingPool.EVSEOperator.RoamingNetwork,
                                                                              null, //ChargingStation.ChargingPool.Id,
                                                                              null, //ChargingStation.Id,
                                                                              Id,
                                                                              ChargingProduct,
                                                                              AuthTokens,
                                                                              eMAIds,
                                                                              PINs);

                            OnNewReservation?.Invoke(DateTime.UtcNow, this, updatedReservation);

                            result = ReservationResult.Success(updatedReservation);

                        }

                        #endregion

                        #region ...or a new reservation

                        else
                        {

                            switch (Status.Value)
                            {

                                case EVSEStatusTypes.OutOfService:
                                    result = ReservationResult.OutOfService;
                                    break;

                                case EVSEStatusTypes.Charging:
                                case EVSEStatusTypes.Reserved:
                                case EVSEStatusTypes.Available:

                                    newReservation = new ChargingReservation(ReservationId:           ReservationId ?? ChargingReservation_Id.Random(OperatorId),
                                                                             Timestamp:               Timestamp.Value,
                                                                             StartTime:               ReservationStartTime ?? DateTime.UtcNow,
                                                                             Duration:                Duration  ?? MaxReservationDuration,
                                                                             EndTime:                 (ReservationStartTime ?? DateTime.UtcNow) + (Duration ?? MaxReservationDuration),
                                                                             ConsumedReservationTime: TimeSpan.FromSeconds(0),
                                                                             ReservationLevel:        ReservationLevel,
                                                                             ProviderId:              ProviderId,
                                                                             Identification:          RemoteAuthentication,
                                                                             RoamingNetwork:          null,
                                                                             ChargingPoolId:          null,
                                                                             ChargingStationId:       null,
                                                                             EVSEId:                  Id,
                                                                             ChargingProduct:         ChargingProduct,
                                                                             AuthTokens:              AuthTokens,
                                                                             eMAIds:                  eMAIds,
                                                                             PINs:                    PINs ?? (new UInt32[] { (UInt32) (_random.Next(1000000) + 100000) }));

                                    _Reservations.Add(newReservation.Id, newReservation);

                                    result = ReservationResult.Success(newReservation);

                                    break;

                                default:
                                    result = ReservationResult.Error();
                                    break;

                            }

                        }

                        #endregion

                    }

                }
                else
                {

                    switch (AdminStatus.Value)
                    {

                        default:
                            result = ReservationResult.OutOfService;
                            break;

                    }

                }


                if (result.Result  == ReservationResultType.Success &&
                    newReservation != null)
                {

                    SetStatus(EVSEStatusTypes.Reserved);

                    OnNewReservation?.Invoke(DateTime.UtcNow,
                                             this,
                                             newReservation);

                }

            }
            catch (Exception e)
            {
                result = ReservationResult.Error(e.Message);
            }


            #region Send OnReserveResponse event

            var EndTime = DateTime.UtcNow;

            try
            {

                OnReserveResponse?.Invoke(EndTime,
                                          Timestamp.Value,
                                          this,
                                          EventTrackingId,
                                          ReservationId,
                                          ChargingLocation,
                                          ReservationStartTime,
                                          Duration,
                                          ProviderId,
                                          RemoteAuthentication,
                                          ChargingProduct,
                                          AuthTokens,
                                          eMAIds,
                                          PINs,
                                          result,
                                          EndTime - StartTime,
                                          RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(VirtualEVSE) + "." + nameof(OnReserveResponse));
            }

            #endregion

            return Task.FromResult(result);

        }

        #endregion

        #region CancelReservation(ReservationId, Reason, ProviderId = null, ...)

        /// <summary>
        /// Try to remove the given charging reservation.
        /// </summary>
        /// <param name="ReservationId">The unique charging reservation identification.</param>
        /// <param name="Reason">A reason for this cancellation.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<CancelReservationResult>

            CancelReservation(ChargingReservation_Id                 ReservationId,
                              ChargingReservationCancellationReason  Reason,
                              eMobilityProvider_Id?                  ProviderId         = null,

                              DateTime?                              Timestamp          = null,
                              CancellationToken?                     CancellationToken  = null,
                              EventTracking_Id                       EventTrackingId    = null,
                              TimeSpan?                              RequestTimeout     = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            ChargingReservation     canceledReservation  = null;
            CancelReservationResult result               = null;

            #endregion

            #region Send OnCancelReservationRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnCancelReservationRequest?.Invoke(StartTime,
                                                   Timestamp.Value,
                                                   this,
                                                   EventTrackingId,
                                                   ProviderId,
                                                   ReservationId,
                                                   Reason,
                                                   RequestTimeout);


            }
            catch (Exception e)
            {
                e.Log(nameof(VirtualEVSE) + "." + nameof(OnCancelReservationRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                    AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
                {

                    lock (_Reservations)
                    {

                        if (!_Reservations.TryGetValue(ReservationId, out canceledReservation))
                            return Task.FromResult(CancelReservationResult.UnknownReservationId(ReservationId,
                                                                                                Reason));

                        _Reservations.Remove(ReservationId);

                    }

                    result = CancelReservationResult.Success(ReservationId,
                                                             Reason,
                                                             canceledReservation);

                }

                else
                {

                    switch (AdminStatus.Value)
                    {

                        default:
                            result = CancelReservationResult.OutOfService(ReservationId,
                                                                          Reason);
                            break;

                    }

                }


                if (result.Result == CancelReservationResultTypes.Success)
                {

                    if (Status.Value == EVSEStatusTypes.Reserved &&
                    !_Reservations.Any())
                    {
                        // Will send events!
                        SetStatus(EVSEStatusTypes.Available);
                    }

                    OnReservationCanceled?.Invoke(DateTime.UtcNow,
                                                  this,
                                                  canceledReservation,
                                                  Reason);

                }


            }
            catch (Exception e)
            {
                result = CancelReservationResult.Error(ReservationId,
                                                       Reason,
                                                       e.Message);
            }


            #region Send OnCancelReservationResponse event

            var EndTime = DateTime.UtcNow;

            try
            {

                OnCancelReservationResponse?.Invoke(EndTime,
                                                    Timestamp.Value,
                                                    this,
                                                    EventTrackingId,
                                                    ProviderId,
                                                    ReservationId,
                                                    canceledReservation,
                                                    Reason,
                                                    result,
                                                    EndTime - StartTime,
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(VirtualEVSE) + "." + nameof(OnCancelReservationResponse));
            }

            #endregion

            return Task.FromResult(result);

        }

        #endregion

        #region CheckIfReservationIsExpired(State)

        /// <summary>
        /// Check if the reservation is expired.
        /// </summary>
        public void CheckIfReservationIsExpired(Object State)
        {

            if (Monitor.TryEnter(ReservationExpiredLock))
            {

                try
                {

                    ChargingReservation[] expiredReservations = null;

                    lock (_Reservations)
                    {
                        expiredReservations = _Reservations.Values.Where(reservation => reservation.IsExpired()).ToArray();
                    }

                    foreach (var expiredReservation in expiredReservations)
                    {

                        lock (_Reservations)
                        {
                            _Reservations.Remove(expiredReservation.Id);
                        }

                        if (Status.Value == EVSEStatusTypes.Reserved &&
                            !_Reservations.Any())
                        {
                            // Will send events!
                            SetStatus(EVSEStatusTypes.Available);
                        }

                        OnReservationCanceled?.Invoke(DateTime.UtcNow,
                                                      this,
                                                      expiredReservation,
                                                      ChargingReservationCancellationReason.Expired);

                    }

                }
                catch (Exception e)
                {
                    DebugX.LogT(e.Message);
                }
                finally
                {
                    Monitor.Exit(ReservationExpiredLock);
                }

            }

        }

        #endregion

        #endregion

        #region RemoteStart/-Stop and Sessions...

        #region Data

        private ChargingSession _ChargingSession;


        public IEnumerable<ChargingSession> ChargingSessions
            => _ChargingSession != null
                   ? new ChargingSession[] { _ChargingSession }
                   : new ChargingSession[0];

        #region TryGetChargingSessionById(SessionId, out ChargingSession)

        /// <summary>
        /// Return the charging session specified by the given identification.
        /// </summary>
        /// <param name="SessionId">The charging session identification.</param>
        /// <param name="ChargingSession">The charging session.</param>
        public Boolean TryGetChargingSessionById(ChargingSession_Id SessionId, out ChargingSession ChargingSession)
        {

            if (SessionId == _ChargingSession.Id)
            {
                ChargingSession = _ChargingSession;
                return true;
            }

            ChargingSession = null;
            return false;

        }

        #endregion

        /// <summary>
        /// The current charging session, if available.
        /// </summary>
        [InternalUseOnly]
        public ChargingSession ChargingSession
        {

            get
            {
                return _ChargingSession;
            }

            set
            {

                // Skip, if the charging session is already known... 
                if (_ChargingSession != value)
                {

                    _ChargingSession = value;

                    if (_ChargingSession != null)
                    {

                        SetStatus(EVSEStatusTypes.Charging);

                        OnNewChargingSession?.Invoke(DateTime.UtcNow, this, _ChargingSession);

                    }

                    else
                        SetStatus(EVSEStatusTypes.Available);

                }

            }

        }

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a remote start command was received.
        /// </summary>
        public event OnRemoteStartRequestDelegate     OnRemoteStartRequest;

        /// <summary>
        /// An event fired whenever a remote start command completed.
        /// </summary>
        public event OnRemoteStartResponseDelegate    OnRemoteStartResponse;

        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        public event OnNewChargingSessionDelegate     OnNewChargingSession;


        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopRequestDelegate      OnRemoteStopRequest;

        /// <summary>
        /// An event fired whenever a remote stop command completed.
        /// </summary>
        public event OnRemoteStopResponseDelegate     OnRemoteStopResponse;

        /// <summary>
        /// An event fired whenever a new charge detail record was created.
        /// </summary>
        public event OnNewChargeDetailRecordDelegate  OnNewChargeDetailRecord;

        #endregion

        #region RemoteStart(                  ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, RemoteAuthentication = null, ...)

        /// <summary>
        /// Start a charging session.
        /// </summary>
        /// <param name="ChargingProduct">The choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<RemoteStartResult>

            RemoteStart(ChargingProduct          ChargingProduct        = null,
                        ChargingReservation_Id?  ReservationId          = null,
                        ChargingSession_Id?      SessionId              = null,
                        eMobilityProvider_Id?    ProviderId             = null,
                        RemoteAuthentication     RemoteAuthentication   = null,

                        DateTime?                Timestamp              = null,
                        CancellationToken?       CancellationToken      = null,
                        EventTracking_Id         EventTrackingId        = null,
                        TimeSpan?                RequestTimeout         = null)


                => RemoteStart(ChargingLocation.FromEVSEId(Id),
                               ChargingProduct,
                               ReservationId,
                               SessionId,
                               ProviderId,
                               RemoteAuthentication,

                               Timestamp,
                               CancellationToken,
                               EventTrackingId,
                               RequestTimeout);

        #endregion

        #region RemoteStart(ChargingLocation, ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, RemoteAuthentication = null, ...)

        /// <summary>
        /// Start a charging session.
        /// </summary>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">The choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<RemoteStartResult>

            RemoteStart(ChargingLocation         ChargingLocation,
                        ChargingProduct          ChargingProduct        = null,
                        ChargingReservation_Id?  ReservationId          = null,
                        ChargingSession_Id?      SessionId              = null,
                        eMobilityProvider_Id?    ProviderId             = null,
                        RemoteAuthentication     RemoteAuthentication   = null,

                        DateTime?                Timestamp              = null,
                        CancellationToken?       CancellationToken      = null,
                        EventTracking_Id         EventTrackingId        = null,
                        TimeSpan?                RequestTimeout         = null)
        {

            #region Initial checks

            if (SessionId == null)
                SessionId = ChargingSession_Id.New;

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            RemoteStartResult result = null;

            #endregion

            #region Send OnRemoteStartRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnRemoteStartRequest?.Invoke(StartTime,
                                             Timestamp.Value,
                                             this,
                                             EventTrackingId,
                                             ChargingLocation,
                                             ChargingProduct,
                                             ReservationId,
                                             SessionId,
                                             ProviderId,
                                             RemoteAuthentication,
                                             RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(VirtualEVSE) + "." + nameof(OnRemoteStartRequest));
            }

            #endregion


            try
            {

                if (ChargingLocation.EVSEId.HasValue &&
                    ChargingLocation.EVSEId.Value != Id)
                {
                    result = RemoteStartResult.UnknownLocation;
                }

                else if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                         AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
                {

                    switch (Status.Value)
                    {

                        #region Available

                        case EVSEStatusTypes.Available:
                        case EVSEStatusTypes.DoorNotClosed:

                            // Will also set the status -> EVSEStatusType.Charging!
                            ChargingSession = new ChargingSession(SessionId.Value)
                            {
                                EventTrackingId = EventTrackingId,
                                Reservation = _Reservations.Values.FirstOrDefault(reservation => reservation.Id == ReservationId),
                                ReservationId = ReservationId,
                                EVSEId = Id,
                                ChargingProduct = ChargingProduct,
                                ProviderIdStart = ProviderId,
                                IdentificationStart = RemoteAuthentication,
                            };

                            ChargingSession.AddEnergyMeterValue(new Timestamped<Single>(DateTime.UtcNow, 0));
                            EnergyMeterTimer.Change(EnergyMeterInterval, EnergyMeterInterval);

                            result = RemoteStartResult.Success(_ChargingSession);
                            break;

                        #endregion

                        #region Reserved

                        case EVSEStatusTypes.Reserved:

                            var firstReservation = _Reservations.Values.OrderBy(reservation => reservation.StartTime).FirstOrDefault();

                            #region Not matching reservation identifications...

                            if (firstReservation != null && !ReservationId.HasValue)
                                result = RemoteStartResult.Reserved("Missing reservation identification!");

                            else if (firstReservation != null && ReservationId.HasValue && firstReservation.Id != ReservationId.Value)
                                result = RemoteStartResult.Reserved("Invalid reservation identification!");

                            #endregion

                            #region ...or a matching reservation identification!

                            // Check if this remote start is allowed!
                            else if (RemoteAuthentication?.RemoteIdentification.HasValue == true &&
                                !firstReservation.eMAIds.Contains(RemoteAuthentication.RemoteIdentification.Value))
                            {
                                result = RemoteStartResult.InvalidCredentials;
                            }

                            else
                            {

                                firstReservation.AddToConsumedReservationTime(firstReservation.Duration - firstReservation.TimeLeft);

                                // Will also set the status -> EVSEStatusType.Charging;
                                ChargingSession = new ChargingSession(SessionId.Value)
                                {
                                    EventTrackingId = EventTrackingId,
                                    Reservation = firstReservation,
                                    ProviderIdStart = ProviderId,
                                    IdentificationStart = RemoteAuthentication,
                                    EVSEId = Id,
                                    ChargingProduct = ChargingProduct
                                };

                                firstReservation.ChargingSession = ChargingSession;

                                ChargingSession.AddEnergyMeterValue(new Timestamped<Single>(DateTime.UtcNow, 0));
                                EnergyMeterTimer.Change(EnergyMeterInterval, EnergyMeterInterval);

                                result = RemoteStartResult.Success(_ChargingSession);

                            }

                            #endregion

                            break;

                        #endregion

                        #region Charging

                        case EVSEStatusTypes.Charging:
                            result = RemoteStartResult.AlreadyInUse;
                            break;

                        #endregion

                        #region OutOfService

                        case EVSEStatusTypes.OutOfService:
                            result = RemoteStartResult.OutOfService;
                            break;

                        #endregion

                        #region Offline

                        case EVSEStatusTypes.Offline:
                            result = RemoteStartResult.Offline;
                            break;

                        #endregion

                        default:
                            result = RemoteStartResult.Error("Could not start charging!");
                            break;

                    }


                }
                else
                {

                    switch (AdminStatus.Value)
                    {

                        default:
                            result = RemoteStartResult.OutOfService;
                            break;

                    }

                }


            } catch (Exception e)
            {
                result = RemoteStartResult.Error(e.Message);
            }


            #region Send OnRemoteStartResponse event

            var EndTime = DateTime.UtcNow;

            try
            {

                OnRemoteStartResponse?.Invoke(EndTime,
                                              Timestamp.Value,
                                              this,
                                              EventTrackingId,
                                              ChargingLocation,
                                              ChargingProduct,
                                              ReservationId,
                                              SessionId,
                                              ProviderId,
                                              RemoteAuthentication,
                                              RequestTimeout,
                                              result,
                                              EndTime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(VirtualEVSE) + "." + nameof(OnRemoteStartResponse));
            }

            #endregion

            return Task.FromResult(result);

        }

        #endregion

        #region RemoteStop (SessionId, ReservationHandling = null, ProviderId = null, RemoteAuthentication = null, ...)

        /// <summary>
        /// Stop the given charging session.
        /// </summary>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<RemoteStopResult>

            RemoteStop(ChargingSession_Id     SessionId,
                       ReservationHandling?   ReservationHandling    = null,
                       eMobilityProvider_Id?  ProviderId             = null,
                       RemoteAuthentication   RemoteAuthentication   = null,

                       DateTime?              Timestamp              = null,
                       CancellationToken?     CancellationToken      = null,
                       EventTracking_Id       EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null)

        {

            #region Initial checks

            if (SessionId == null)
                SessionId = ChargingSession_Id.New;

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            RemoteStopResult result = null;

            #endregion

            #region Send OnRemoteStopRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnRemoteStopRequest?.Invoke(StartTime,
                                            Timestamp.Value,
                                            this,
                                            EventTrackingId,
                                            SessionId,
                                            ReservationHandling,
                                            ProviderId,
                                            RemoteAuthentication,
                                            RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(VirtualEVSE) + "." + nameof(OnRemoteStopRequest));
            }

            #endregion


            try {

                if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                    AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
                {

                    switch (Status.Value)
                    {

                        #region Available

                        case EVSEStatusTypes.Available:
                            result = RemoteStopResult.InvalidSessionId(SessionId);
                            break;

                        #endregion

                        #region Reserved

                        case EVSEStatusTypes.Reserved:
                            result = RemoteStopResult.InvalidSessionId(SessionId);
                            break;

                        #endregion

                        #region Charging

                        case EVSEStatusTypes.Charging:

                            #region Matching session identification...

                            if (ChargingSession.Id == SessionId)
                            {

                                EnergyMeterTimer.Change(Timeout.Infinite, Timeout.Infinite);

                                var Now                  = DateTime.UtcNow;
                                var Duration             = Now - _ChargingSession.SessionTime.StartTime;
                                var Consumption          = (Single) Math.Round(Duration.TotalHours * MaxPower, 2);

                                ChargingSession.AddEnergyMeterValue(new Timestamped<Single>(Now, 0));

                                var _ChargeDetailRecord  = new ChargeDetailRecord(SessionId:                 _ChargingSession.Id,
                                                                                  Reservation:               _ChargingSession.Reservation,
                                                                                  EVSEId:                    _ChargingSession.EVSEId,
                                                                                  EVSE:                      _ChargingSession.EVSE,
                                                                                  ChargingStation:           _ChargingSession.EVSE?.ChargingStation,
                                                                                  ChargingPool:              _ChargingSession.EVSE?.ChargingStation?.ChargingPool,
                                                                                  ChargingStationOperator:   _ChargingSession.EVSE?.Operator,
                                                                                  ChargingProduct:           _ChargingSession.ChargingProduct,
                                                                                    ProviderIdStart:           _ChargingSession.ProviderIdStart,
                                                                                    ProviderIdStop:            _ChargingSession.ProviderIdStop,
                                                                                  SessionTime:               _ChargingSession.SessionTime,

                                                                                    IdentificationStart:       _ChargingSession.IdentificationStart,
                                                                                    IdentificationStop:        _ChargingSession.IdentificationStop,

                                                                                  EnergyMeterId:             EnergyMeterId,
                                                                                  EnergyMeteringValues:      ChargingSession.EnergyMeteringValues
                                                                                  //SignedMeteringValues:      ChargingSession.EnergyMeteringValues.Select(metervalue =>
                                                                                  //                               new SignedMeteringValue(metervalue.Timestamp,
                                                                                  //                                                       metervalue.Value,
                                                                                  //                                                       EnergyMeterId.Value,
                                                                                  //                                                       Id,
                                                                                  //                                                       _ChargingSession.IdentificationStart,
                                                                                  //                                                       PublicKeyRing.First()).
                                                                                  //                                   Sign(SecretKeyRing.First(),
                                                                                  //                                        Passphrase))
                                                                                                                   //                      PublicKeys.First().First()).
                                                                                                                   //  Sign(SecretKeys.First().First(),
                                                                                                                   //       Passphrase),


                                                                                 );

                                // Will do: Status = EVSEStatusType.Available
                                ChargingSession = null;

                                if (!ReservationHandling.HasValue ||
                                    (ReservationHandling.HasValue &&
                                    !ReservationHandling.Value.IsKeepAlive))
                                {

                                    // Will do: Status = EVSEStatusType.Available
                                    //Reservation = null;

                                }

                                else
                                {
                                    //ToDo: Reservation will live on!
                                }


                                OnNewChargeDetailRecord?.Invoke(DateTime.UtcNow,
                                                                this,
                                                                _ChargeDetailRecord);

                                result = RemoteStopResult.Success(_ChargeDetailRecord,
                                                                  _ChargingSession.Reservation?.Id,
                                                                  ReservationHandling);

                            }

                            #endregion

                            #region ...or unknown session identification!

                            else
                                result = RemoteStopResult.InvalidSessionId(SessionId);

                            #endregion

                            break;

                        #endregion

                        #region OutOfService

                        case EVSEStatusTypes.OutOfService:
                            result = RemoteStopResult.OutOfService(SessionId);
                            break;

                        #endregion

                        #region Offline

                        case EVSEStatusTypes.Offline:
                            result = RemoteStopResult.Offline(SessionId);
                            break;

                        #endregion

                        default:
                            result = RemoteStopResult.Error(SessionId);
                            break;

                    }

                }
                else
                {

                    switch (AdminStatus.Value)
                    {

                        default:
                            result = RemoteStopResult.OutOfService(SessionId);
                            break;

                    }

                }

            }
            catch (Exception e)
            {
                result = RemoteStopResult.Error(SessionId,
                                                e.Message);
            }


            #region Send OnRemoteStopResponse event

            var EndTime = DateTime.UtcNow;

            try
            {

                OnRemoteStopResponse?.Invoke(EndTime,
                                             Timestamp.Value,
                                             this,
                                             EventTrackingId,
                                             SessionId,
                                             ReservationHandling,
                                             ProviderId,
                                             RemoteAuthentication,
                                             RequestTimeout,
                                             result,
                                             EndTime - StartTime);

            }
            catch (Exception e)
            {
                e.Log(nameof(VirtualEVSE) + "." + nameof(OnRemoteStopResponse));
            }

            #endregion

            return Task.FromResult(result);

        }

        #endregion

        #endregion


        #region IEnumerable<SocketOutlet> Members

        /// <summary>
        /// Return a socket outlet enumerator.
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => _SocketOutlets.GetEnumerator();

        /// <summary>
        /// Return a socket outlet enumerator.
        /// </summary>
        public IEnumerator<SocketOutlet> GetEnumerator()
            => _SocketOutlets.GetEnumerator();

        #endregion


        #region Operator overloading

        #region Operator == (VirtualEVSE1, VirtualEVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualEVSE1">A virtual EVSE.</param>
        /// <param name="VirtualEVSE2">Another virtual EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (VirtualEVSE VirtualEVSE1, VirtualEVSE VirtualEVSE2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(VirtualEVSE1, VirtualEVSE2))
                return true;

            // If one is null, but not both, return false.
            if ((VirtualEVSE1 is null) || (VirtualEVSE2 is null))
                return false;

            return VirtualEVSE1.Equals(VirtualEVSE2);

        }

        #endregion

        #region Operator != (VirtualEVSE1, VirtualEVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualEVSE1">A virtual EVSE.</param>
        /// <param name="VirtualEVSE2">Another virtual EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (VirtualEVSE VirtualEVSE1, VirtualEVSE VirtualEVSE2)
            => !(VirtualEVSE1 == VirtualEVSE2);

        #endregion

        #region Operator <  (VirtualEVSE1, VirtualEVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualEVSE1">A virtual EVSE.</param>
        /// <param name="VirtualEVSE2">Another virtual EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (VirtualEVSE VirtualEVSE1, VirtualEVSE VirtualEVSE2)
        {

            if (VirtualEVSE1 is null)
                throw new ArgumentNullException(nameof(VirtualEVSE1), "The given VirtualEVSE1 must not be null!");

            return VirtualEVSE1.CompareTo(VirtualEVSE2) < 0;

        }

        #endregion

        #region Operator <= (VirtualEVSE1, VirtualEVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualEVSE1">A virtual EVSE.</param>
        /// <param name="VirtualEVSE2">Another virtual EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (VirtualEVSE VirtualEVSE1, VirtualEVSE VirtualEVSE2)
            => !(VirtualEVSE1 > VirtualEVSE2);

        #endregion

        #region Operator >  (VirtualEVSE1, VirtualEVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualEVSE1">A virtual EVSE.</param>
        /// <param name="VirtualEVSE2">Another virtual EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (VirtualEVSE VirtualEVSE1, VirtualEVSE VirtualEVSE2)
        {

            if (VirtualEVSE1 is null)
                throw new ArgumentNullException(nameof(VirtualEVSE1), "The given VirtualEVSE1 must not be null!");

            return VirtualEVSE1.CompareTo(VirtualEVSE2) > 0;

        }

        #endregion

        #region Operator >= (VirtualEVSE1, VirtualEVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualEVSE1">A virtual EVSE.</param>
        /// <param name="VirtualEVSE2">Another virtual EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (VirtualEVSE VirtualEVSE1, VirtualEVSE VirtualEVSE2)
            => !(VirtualEVSE1 < VirtualEVSE2);

        #endregion

        #endregion

        #region IComparable<VirtualEVSE> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is VirtualEVSE VirtualEVSE))
                throw new ArgumentException("The given object is not a virtual EVSE!");

            return CompareTo(VirtualEVSE);

        }

        #endregion

        #region CompareTo(VirtualEVSE)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualEVSE">An virtual EVSE to compare with.</param>
        public Int32 CompareTo(VirtualEVSE VirtualEVSE)
        {

            if (VirtualEVSE is null)
                throw new ArgumentNullException(nameof(VirtualEVSE),  "The given virtual EVSE must not be null!");

            return Id.CompareTo(VirtualEVSE.Id);

        }

        #endregion

        #endregion

        #region IEquatable<VirtualEVSE> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            if (!(Object is VirtualEVSE VirtualEVSE))
                return false;

            return Equals(VirtualEVSE);

        }

        #endregion

        #region Equals(VirtualEVSE)

        /// <summary>
        /// Compares two virtual EVSEs for equality.
        /// </summary>
        /// <param name="VirtualEVSE">A virtual EVSE to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(VirtualEVSE VirtualEVSE)
        {

            if (VirtualEVSE is null)
                return false;

            return Id.Equals(VirtualEVSE.Id);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()

            => Id.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => Id.ToString();

        #endregion

    }

}
