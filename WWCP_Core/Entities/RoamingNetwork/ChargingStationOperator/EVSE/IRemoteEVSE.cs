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
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace org.GraphDefined.WWCP
{

    public static class IRemoteEVSEExtentions
    {


        #region Reserve(...StartTime, Duration, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at the given EVSE.
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
        public static Task<ReservationResult>

            Reserve(this IRemoteEVSE                  RemoteEVSE,
                    DateTime?                         StartTime              = null,
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


                => RemoteEVSE.Reserve(ChargingReservationLevel.EVSE,
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

    }



    /// <summary>
    /// The interface of a remote EVSE.
    /// </summary>
    public interface IRemoteEVSE
    {

        #region Properties

        /// <summary>
        /// The unique identification of this EVSE.
        /// </summary>
        EVSE_Id                     Id                      { get; }

        /// <summary>
        /// An description of this EVSE.
        /// </summary>
        I18NString                  Description             { get; set; }


        /// <summary>
        /// Charging modes.
        /// </summary>
        ReactiveSet<ChargingModes>  ChargingModes           { get; set; }

        /// <summary>
        /// The average voltage.
        /// </summary>
        Double                      AverageVoltage          { get; set; }

        /// <summary>
        /// The type of the current.
        /// </summary>
        CurrentTypes                CurrentType             { get; set; }

        /// <summary>
        /// The maximum current [Ampere].
        /// </summary>
        Double                      MaxCurrent              { get; set; }

        /// <summary>
        /// The maximum power [kWatt].
        /// </summary>
        Double                      MaxPower                { get; set; }

        /// <summary>
        /// The current real-time power delivery [Watt].
        /// </summary>
        Double                      RealTimePower           { get; set; }

        /// <summary>
        /// The maximum capacity [kWh].
        /// </summary>
        Double?                     MaxCapacity             { get; set; }

        /// <summary>
        /// The energy meter identification.
        /// </summary>
        EnergyMeter_Id?             EnergyMeterId           { get; set; }


        ReactiveSet<SocketOutlet> SocketOutlets { get; set; }

        #endregion

        #region Events

        #region OnEVSEData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE changed.
        /// </summary>
        event OnRemoteEVSEDataChangedDelegate         OnDataChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        event OnRemoteEVSEAdminStatusChangedDelegate  OnAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        event OnRemoteEVSEStatusChangedDelegate       OnStatusChanged;

        #endregion


        /// <summary>
        /// An event fired whenever a new charging reservation was created.
        /// </summary>
        event OnNewReservationDelegate      OnNewReservation;


        /// <summary>
        /// An event fired whenever a charging reservation was cancelled.
        /// </summary>
        event OnCancelReservationResponseDelegate OnReservationCancelled;


        /// <summary>
        /// An event fired whenever a new charge detail record was created.
        /// </summary>
        event OnNewChargeDetailRecordDelegate OnNewChargeDetailRecord;

        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        event OnNewChargingSessionDelegate  OnNewChargingSession;

        #endregion


        #region (Admin-)Status

        void SetAdminStatus(EVSEAdminStatusTypes NewAdminStatus);
        void SetAdminStatus(Timestamped<EVSEAdminStatusTypes> NewTimestampedAdminStatus);
        void SetAdminStatus(IEnumerable<Timestamped<EVSEAdminStatusTypes>> NewAdminStatusList, ChangeMethods ChangeMethod = ChangeMethods.Replace);
        void SetAdminStatus(EVSEAdminStatusTypes NewAdminStatus, DateTime Timestamp);

        Timestamped<EVSEAdminStatusTypes>              AdminStatus         { get; set; }
        IEnumerable<Timestamped<EVSEAdminStatusTypes>> AdminStatusSchedule { get; }


        void SetStatus(EVSEStatusTypes NewStatus);
        void SetStatus(Timestamped<EVSEStatusTypes> NewTimestampedStatus);
        void SetStatus(IEnumerable<Timestamped<EVSEStatusTypes>> NewStatusList, ChangeMethods ChangeMethod = ChangeMethods.Replace);
        void SetStatus(EVSEStatusTypes NewStatus, DateTime Timestamp);

        Timestamped<EVSEStatusTypes>              Status         { get; set; }
        IEnumerable<Timestamped<EVSEStatusTypes>> StatusSchedule { get; }

        #endregion

        #region Reservations

        /// <summary>
        /// Reserve the possibility to charge.
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
        Task<ReservationResult>

            Reserve(ChargingReservationLevel          ChargingReservationLevel   = ChargingReservationLevel.EVSE,
                    DateTime?                         StartTime                  = null,
                    TimeSpan?                         Duration                   = null,
                    ChargingReservation_Id?           ReservationId              = null,
                    eMobilityProvider_Id?             ProviderId                 = null,
                    RemoteAuthentication              RemoteAuthentication       = null,
                    ChargingProduct                   ChargingProduct            = null,
                    IEnumerable<Auth_Token>           AuthTokens                 = null,
                    IEnumerable<eMobilityAccount_Id>  eMAIds                     = null,
                    IEnumerable<UInt32>               PINs                       = null,

                    DateTime?                         Timestamp                  = null,
                    CancellationToken?                CancellationToken          = null,
                    EventTracking_Id                  EventTrackingId            = null,
                    TimeSpan?                         RequestTimeout             = null);


        /// <summary>
        /// Try to remove the given charging reservation.
        /// </summary>
        /// <param name="ReservationId">The unique charging reservation identification.</param>
        /// <param name="Reason">A reason for this cancellation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<CancelReservationResult>

            CancelReservation(ChargingReservation_Id                 ReservationId,
                              ChargingReservationCancellationReason  Reason,
                              eMobilityProvider_Id?                  ProviderId          = null,

                              DateTime?                              Timestamp           = null,
                              CancellationToken?                     CancellationToken   = null,
                              EventTracking_Id                       EventTrackingId     = null,
                              TimeSpan?                              RequestTimeout      = null);

        ChargingReservation Reservation { get; set; }

        #endregion

        #region RemoteStart/-Stop

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
        Task<RemoteStartEVSEResult>

            RemoteStart(ChargingProduct          ChargingProduct,
                        ChargingReservation_Id?  ReservationId,
                        ChargingSession_Id?      SessionId,
                        eMobilityProvider_Id?    ProviderId             = null,
                        RemoteAuthentication     RemoteAuthentication   = null,

                        DateTime?                Timestamp              = null,
                        CancellationToken?       CancellationToken      = null,
                        EventTracking_Id         EventTrackingId        = null,
                        TimeSpan?                RequestTimeout         = null);


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
        Task<RemoteStopEVSEResult>

            RemoteStop(ChargingSession_Id     SessionId,
                       ReservationHandling?   ReservationHandling    = null,
                       eMobilityProvider_Id?  ProviderId             = null,
                       RemoteAuthentication   RemoteAuthentication   = null,

                       DateTime?              Timestamp              = null,
                       CancellationToken?     CancellationToken      = null,
                       EventTracking_Id       EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null);

        ChargingSession ChargingSession { get; set; }

        #endregion


        Task CheckIfReservationIsExpired();

        IRemoteChargingStation ChargingStation { get; }
        ChargingStationOperator_Id OperatorId { get; }

        IEnumerator<SocketOutlet> GetEnumerator();

        Int32   CompareTo(Object Object);
        Boolean Equals(Object Object);

        Int32   GetHashCode();
        String  ToString();

    }

}