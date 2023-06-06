﻿/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Mail;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

using social.OpenData.UsersAPI;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A delegate for filtering charging station operator identifications.
    /// </summary>
    /// <param name="ChargingStationOperatorId">A charging station operator identification to include.</param>
    public delegate Boolean IncludeChargingStationOperatorIdDelegate(ChargingStationOperator_Id  ChargingStationOperatorId);

    /// <summary>
    /// A delegate for filtering charging station operators.
    /// </summary>
    /// <param name="ChargingStationOperator">A charging station operator to include.</param>
    public delegate Boolean IncludeChargingStationOperatorDelegate  (IChargingStationOperator     ChargingStationOperator);


    /// <summary>
    /// Extension methods for charging station operators.
    /// </summary>
    public static class IChargingStationOperatorExtensions
    {

        #region AddChargingPool           (this IChargingStationOperator, Id = null, Name = null, ...)

        /// <summary>
        /// Add a new charging pool, but do not fail when this charging pool already exists.
        /// </summary>
        /// <param name="ChargingStationOperator">The charging station operator of the new charging pool.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to be called after the successful addition of the charging pool.</param>
        /// <param name="OnError">An optional delegate to be called whenever the addition of the new charging pool failed.</param>
        /// 
        /// <param name="SkipAddedNotifications">Whether to skip sending the 'OnAdded' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public static Task<AddChargingPoolResult> AddChargingPool(this IChargingStationOperator                                       ChargingStationOperator,

                                                                  ChargingPool_Id?                                                    Id                             = null,
                                                                  I18NString?                                                         Name                           = null,
                                                                  I18NString?                                                         Description                    = null,

                                                                  Address?                                                            Address                        = null,
                                                                  GeoCoordinate?                                                      GeoLocation                    = null,
                                                                  OpeningTimes?                                                       OpeningTimes                   = null,
                                                                  Boolean?                                                            ChargingWhenClosed             = null,
                                                                  AccessibilityTypes?                                                 Accessibility                  = null,
                                                                  Languages?                                                          LocationLanguage               = null,
                                                                  PhoneNumber?                                                        HotlinePhoneNumber             = null,

                                                                  Timestamped<ChargingPoolAdminStatusTypes>?                          InitialAdminStatus             = null,
                                                                  Timestamped<ChargingPoolStatusTypes>?                               InitialStatus                  = null,
                                                                  UInt16?                                                             MaxAdminStatusScheduleSize     = null,
                                                                  UInt16?                                                             MaxStatusScheduleSize          = null,

                                                                  String?                                                             DataSource                     = null,
                                                                  DateTime?                                                           LastChange                     = null,

                                                                  JObject?                                                            CustomData                     = null,
                                                                  UserDefinedDictionary?                                              InternalData                   = null,

                                                                  Action<IChargingPool>?                                              Configurator                   = null,
                                                                  RemoteChargingPoolCreatorDelegate?                                  RemoteChargingPoolCreator      = null,

                                                                  Action<IChargingPool, EventTracking_Id>?                            OnSuccess                      = null,
                                                                  Action<IChargingStationOperator, IChargingPool, EventTracking_Id>?  OnError                        = null,

                                                                  Boolean                                                             SkipAddedNotifications         = false,
                                                                  Func<ChargingStationOperator_Id, ChargingPool_Id, Boolean>?         AllowInconsistentOperatorIds   = null,
                                                                  EventTracking_Id?                                                   EventTrackingId                = null,
                                                                  User_Id?                                                            CurrentUserId                  = null)


            => ChargingStationOperator.AddChargingPool(new ChargingPool(
                                                           Id ?? ChargingPool_Id.NewRandom(ChargingStationOperator.Id),
                                                           ChargingStationOperator,
                                                           Name,
                                                           Description,

                                                           Address,
                                                           GeoLocation,
                                                           OpeningTimes,
                                                           ChargingWhenClosed,
                                                           Accessibility,
                                                           LocationLanguage,
                                                           HotlinePhoneNumber,

                                                           InitialAdminStatus,
                                                           InitialStatus,
                                                           MaxAdminStatusScheduleSize,
                                                           MaxStatusScheduleSize,

                                                           DataSource,
                                                           LastChange,

                                                           CustomData,
                                                           InternalData,

                                                           Configurator,
                                                           RemoteChargingPoolCreator
                                                       ),

                                                       OnSuccess,
                                                       OnError,

                                                       SkipAddedNotifications,
                                                       AllowInconsistentOperatorIds,
                                                       EventTrackingId,
                                                       CurrentUserId);

        #endregion

        #region AddChargingPoolIfNotExists(this IChargingStationOperator, Id = null, Name = null, ...)

        /// <summary>
        /// Add a new charging pool, but do not fail when this charging pool already exists.
        /// </summary>
        /// <param name="ChargingStationOperator">The charging station operator of the new charging pool.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to be called after the successful addition of the charging pool.</param>
        /// 
        /// <param name="SkipAddedNotifications">Whether to skip sending the 'OnAdded' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public static Task<AddChargingPoolResult> AddChargingPoolIfNotExists(this IChargingStationOperator                                ChargingStationOperator,

                                                                             ChargingPool_Id?                                             Id                             = null,
                                                                             I18NString?                                                  Name                           = null,
                                                                             I18NString?                                                  Description                    = null,

                                                                             Address?                                                     Address                        = null,
                                                                             GeoCoordinate?                                               GeoLocation                    = null,
                                                                             OpeningTimes?                                                OpeningTimes                   = null,
                                                                             Boolean?                                                     ChargingWhenClosed             = null,
                                                                             AccessibilityTypes?                                          Accessibility                  = null,
                                                                             Languages?                                                   LocationLanguage               = null,
                                                                             PhoneNumber?                                                 HotlinePhoneNumber             = null,

                                                                             Timestamped<ChargingPoolAdminStatusTypes>?                   InitialAdminStatus             = null,
                                                                             Timestamped<ChargingPoolStatusTypes>?                        InitialStatus                  = null,
                                                                             UInt16?                                                      MaxAdminStatusScheduleSize         = null,
                                                                             UInt16?                                                      MaxStatusScheduleSize              = null,

                                                                             String?                                                      DataSource                     = null,
                                                                             DateTime?                                                    LastChange                     = null,

                                                                             JObject?                                                     CustomData                     = null,
                                                                             UserDefinedDictionary?                                       InternalData                   = null,

                                                                             Action<IChargingPool>?                                       Configurator                   = null,
                                                                             RemoteChargingPoolCreatorDelegate?                           RemoteChargingPoolCreator      = null,

                                                                             Action<IChargingPool, EventTracking_Id>?                     OnSuccess                      = null,

                                                                             Boolean                                                      SkipAddedNotifications         = false,
                                                                             Func<ChargingStationOperator_Id, ChargingPool_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                                                             EventTracking_Id?                                            EventTrackingId                = null,
                                                                             User_Id?                                                     CurrentUserId                  = null)


            => ChargingStationOperator.AddChargingPoolIfNotExists(new ChargingPool(
                                                                      Id ?? ChargingPool_Id.NewRandom(ChargingStationOperator.Id),
                                                                      ChargingStationOperator,
                                                                      Name,
                                                                      Description,

                                                                      Address,
                                                                      GeoLocation,
                                                                      OpeningTimes,
                                                                      ChargingWhenClosed,
                                                                      Accessibility,
                                                                      LocationLanguage,
                                                                      HotlinePhoneNumber,

                                                                      InitialAdminStatus,
                                                                      InitialStatus,
                                                                      MaxAdminStatusScheduleSize,
                                                                      MaxStatusScheduleSize,

                                                                      DataSource,
                                                                      LastChange,

                                                                      CustomData,
                                                                      InternalData,

                                                                      Configurator,
                                                                      RemoteChargingPoolCreator
                                                                  ),

                                                                  OnSuccess,

                                                                  SkipAddedNotifications,
                                                                  AllowInconsistentOperatorIds,
                                                                  EventTrackingId,
                                                                  CurrentUserId);

        #endregion

        #region AddOrUpdateChargingPool   (this IChargingStationOperator, Id,        Name = null, ...)

        /// <summary>
        /// Add a new or update an existing charging pool.
        /// </summary>
        /// <param name="ChargingStationOperator">The charging station operator of the new or updated charging pool.</param>
        /// 
        /// <param name="OnAdditionSuccess">An optional delegate to be called after the successful addition of the charging pool.</param>
        /// <param name="OnUpdateSuccess">An optional delegate to be called after the successful update of the charging pool.</param>
        /// <param name="OnError">An optional delegate to be called whenever the addition of the new charging pool failed.</param>
        /// 
        /// <param name="SkipAddOrUpdatedUpdatedNotifications">Whether to skip sending the 'OnAddedOrUpdated' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public static Task<AddOrUpdateChargingPoolResult> AddOrUpdateChargingPool(this IChargingStationOperator                                       ChargingStationOperator,

                                                                                  ChargingPool_Id                                                     Id,
                                                                                  I18NString?                                                         Name                                   = null,
                                                                                  I18NString?                                                         Description                            = null,

                                                                                  Address?                                                            Address                                = null,
                                                                                  GeoCoordinate?                                                      GeoLocation                            = null,
                                                                                  OpeningTimes?                                                       OpeningTimes                           = null,
                                                                                  Boolean?                                                            ChargingWhenClosed                     = null,
                                                                                  AccessibilityTypes?                                                 Accessibility                          = null,
                                                                                  Languages?                                                          LocationLanguage                       = null,
                                                                                  PhoneNumber?                                                        HotlinePhoneNumber                     = null,

                                                                                  Timestamped<ChargingPoolAdminStatusTypes>?                          InitialAdminStatus                     = null,
                                                                                  Timestamped<ChargingPoolStatusTypes>?                               InitialStatus                          = null,
                                                                                  UInt16?                                                             MaxAdminStatusScheduleSize                 = null,
                                                                                  UInt16?                                                             MaxStatusScheduleSize                      = null,

                                                                                  String?                                                             DataSource                             = null,
                                                                                  DateTime?                                                           LastChange                             = null,

                                                                                  JObject?                                                            CustomData                             = null,
                                                                                  UserDefinedDictionary?                                              InternalData                           = null,

                                                                                  Action<IChargingPool>?                                              Configurator                           = null,
                                                                                  RemoteChargingPoolCreatorDelegate?                                  RemoteChargingPoolCreator              = null,

                                                                                  Action<IChargingPool,                           EventTracking_Id>?  OnAdditionSuccess                      = null,
                                                                                  Action<IChargingPool,            IChargingPool, EventTracking_Id>?  OnUpdateSuccess                        = null,
                                                                                  Action<IChargingStationOperator, IChargingPool, EventTracking_Id>?  OnError                                = null,

                                                                                  Boolean                                                             SkipAddOrUpdatedUpdatedNotifications   = false,
                                                                                  Func<ChargingStationOperator_Id, ChargingPool_Id, Boolean>?         AllowInconsistentOperatorIds           = null,
                                                                                  EventTracking_Id?                                                   EventTrackingId                        = null,
                                                                                  User_Id?                                                            CurrentUserId                          = null)


            => ChargingStationOperator.AddOrUpdateChargingPool(new ChargingPool(
                                                                   Id,
                                                                   ChargingStationOperator,
                                                                   Name,
                                                                   Description,

                                                                   Address,
                                                                   GeoLocation,
                                                                   OpeningTimes,
                                                                   ChargingWhenClosed,
                                                                   Accessibility,
                                                                   LocationLanguage,
                                                                   HotlinePhoneNumber,

                                                                   InitialAdminStatus,
                                                                   InitialStatus,
                                                                   MaxAdminStatusScheduleSize,
                                                                   MaxStatusScheduleSize,

                                                                   DataSource,
                                                                   LastChange,

                                                                   CustomData,
                                                                   InternalData,

                                                                   Configurator,
                                                                   RemoteChargingPoolCreator
                                                               ),

                                                               OnAdditionSuccess,
                                                               OnUpdateSuccess,
                                                               OnError,

                                                               SkipAddOrUpdatedUpdatedNotifications,
                                                               AllowInconsistentOperatorIds,
                                                               EventTrackingId,
                                                               CurrentUserId);

        #endregion

        #region UpdateChargingPool        (this IChargingStationOperator, Id,        Name = null, ...)

        /// <summary>
        /// Update the given charging pool.
        /// </summary>
        /// <param name="ChargingStationOperator">The charging station operator of the updated charging pool.</param>
        /// 
        /// <param name="OnUpdateSuccess">An optional delegate to be called after the successful update of the charging pool.</param>
        /// <param name="OnError">An optional delegate to be called whenever the addition of the new charging pool failed.</param>
        /// 
        /// <param name="SkipUpdatedNotifications">Whether to skip sending the 'OnUpdated' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public static Task<UpdateChargingPoolResult> UpdateChargingPool(this IChargingStationOperator                                       ChargingStationOperator,

                                                                        ChargingPool_Id                                                     Id,
                                                                        I18NString?                                                         Name                           = null,
                                                                        I18NString?                                                         Description                    = null,

                                                                        Address?                                                            Address                        = null,
                                                                        GeoCoordinate?                                                      GeoLocation                    = null,
                                                                        OpeningTimes?                                                       OpeningTimes                   = null,
                                                                        Boolean?                                                            ChargingWhenClosed             = null,
                                                                        AccessibilityTypes?                                                 Accessibility                  = null,
                                                                        Languages?                                                          LocationLanguage               = null,
                                                                        PhoneNumber?                                                        HotlinePhoneNumber             = null,

                                                                        Timestamped<ChargingPoolAdminStatusTypes>?                          InitialAdminStatus             = null,
                                                                        Timestamped<ChargingPoolStatusTypes>?                               InitialStatus                  = null,
                                                                        UInt16?                                                             MaxAdminStatusScheduleSize         = null,
                                                                        UInt16?                                                             MaxStatusScheduleSize              = null,

                                                                        String?                                                             DataSource                     = null,
                                                                        DateTime?                                                           LastChange                     = null,

                                                                        JObject?                                                            CustomData                     = null,
                                                                        UserDefinedDictionary?                                              InternalData                   = null,

                                                                        Action<IChargingPool>?                                              Configurator                   = null,
                                                                        RemoteChargingPoolCreatorDelegate?                                  RemoteChargingPoolCreator      = null,

                                                                        Action<IChargingPool,            IChargingPool, EventTracking_Id>?  OnUpdateSuccess                = null,
                                                                        Action<IChargingStationOperator, IChargingPool, EventTracking_Id>?  OnError                        = null,

                                                                        Boolean                                                             SkipUpdatedNotifications       = false,
                                                                        Func<ChargingStationOperator_Id, ChargingPool_Id, Boolean>?         AllowInconsistentOperatorIds   = null,
                                                                        EventTracking_Id?                                                   EventTrackingId                = null,
                                                                        User_Id?                                                            CurrentUserId                  = null)


            => ChargingStationOperator.UpdateChargingPool(new ChargingPool(
                                                              Id,
                                                              ChargingStationOperator,
                                                              Name,
                                                              Description,

                                                              Address,
                                                              GeoLocation,
                                                              OpeningTimes,
                                                              ChargingWhenClosed,
                                                              Accessibility,
                                                              LocationLanguage,
                                                              HotlinePhoneNumber,

                                                              InitialAdminStatus,
                                                              InitialStatus,
                                                              MaxAdminStatusScheduleSize,
                                                              MaxStatusScheduleSize,

                                                              DataSource,
                                                              LastChange,

                                                              CustomData,
                                                              InternalData,

                                                              Configurator,
                                                              RemoteChargingPoolCreator
                                                          ),

                                                          OnUpdateSuccess,
                                                          OnError,

                                                          SkipUpdatedNotifications,
                                                          AllowInconsistentOperatorIds,
                                                          EventTrackingId,
                                                          CurrentUserId);

        #endregion


        #region ToJSON(this ChargingStationOperators, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of charging station operators.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// <param name="Skip">The optional number of charging station operators to skip.</param>
        /// <param name="Take">The optional number of charging station operators to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a roaming network.</param>
        public static JArray ToJSON(this IEnumerable<IChargingStationOperator>                  ChargingStationOperators,
                                    UInt64?                                                     Skip                                      = null,
                                    UInt64?                                                     Take                                      = null,
                                    Boolean                                                     Embedded                                  = false,
                                    InfoStatus                                                  ExpandRoamingNetworkId                    = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                  ExpandChargingPoolIds                     = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                  ExpandChargingStationIds                  = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                  ExpandEVSEIds                             = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                  ExpandBrandIds                            = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                  ExpandDataLicenses                        = InfoStatus.ShowIdOnly,
                                    CustomJObjectSerializerDelegate<IChargingStationOperator>?  CustomChargingStationOperatorSerializer   = null,
                                    CustomJObjectSerializerDelegate<IChargingPool>?             CustomChargingPoolSerializer              = null,
                                    CustomJObjectSerializerDelegate<IChargingStation>?          CustomChargingStationSerializer           = null,
                                    CustomJObjectSerializerDelegate<IEVSE>?                     CustomEVSESerializer                      = null)


            => ChargingStationOperators?.Any() == true

                   ? new JArray(ChargingStationOperators.
                                    Where         (cso => cso is not null).
                                    OrderBy       (cso => cso.Id).
                                    SkipTakeFilter(Skip, Take).
                                    SafeSelect    (cso => cso.ToJSON(Embedded,
                                                                     ExpandRoamingNetworkId,
                                                                     ExpandChargingPoolIds,
                                                                     ExpandChargingStationIds,
                                                                     ExpandEVSEIds,
                                                                     ExpandBrandIds,
                                                                     ExpandDataLicenses,
                                                                     CustomChargingStationOperatorSerializer,
                                                                     CustomChargingPoolSerializer,
                                                                     CustomChargingStationSerializer,
                                                                     CustomEVSESerializer)).
                                    Where         (cso => cso is not null))

                   : new JArray();

        #endregion


    }


    /// <summary>
    /// The common charging station operator interface.
    /// </summary>
    public interface IChargingStationOperator : IEntity<ChargingStationOperator_Id>,
                                                IAdminStatus<ChargingStationOperatorAdminStatusTypes>,
                                                IStatus<ChargingStationOperatorStatusTypes>,
                                                IRemoteStartStop,
                                                IChargingReservations,
                                                IChargingSessions,
                                                IChargeDetailRecords,
                                                IEnumerable<IChargingPool>,
                                                IEquatable<ChargingStationOperator>,
                                                IComparable<ChargingStationOperator>,
                                                IComparable
    {

        #region Properties

        /// <summary>
        /// The roaming network of this charging pool.
        /// </summary>
        IRoamingNetwork?                RoamingNetwork                   { get; }

        /// <summary>
        /// The remote charging station operator.
        /// </summary>
        IRemoteChargingStationOperator  RemoteChargingStationOperator    { get; }

        /// <summary>
        /// The roaming provider of this charging station operator.
        /// </summary>
        ICSORoamingProvider             EMPRoamingProvider               { get; }


        /// <summary>
        /// The logo of this evse operator.
        /// </summary>
        URL?                            Logo                             { get; set; }

        /// <summary>
        /// All brands registered for this charging pool.
        /// </summary>
        ReactiveSet<Brand>              Brands                           { get; }

        /// <summary>
        /// The license of the charging station operator data.
        /// </summary>
        ReactiveSet<OpenDataLicense>    DataLicenses                     { get; }

        /// <summary>
        /// The address of the operators headquarter.
        /// </summary>
        Address                         Address                          { get; set; }

        /// <summary>
        /// The geographical location of this operator.
        /// </summary>
        GeoCoordinate                   GeoLocation                      { get; set; }

        /// <summary>
        /// The telephone number of the operator's (sales) office.
        /// </summary>
        PhoneNumber?                    Telephone                        { get; set; }

        /// <summary>
        /// The e-mail address of the operator's (sales) office.
        /// </summary>
        SimpleEMailAddress?             EMailAddress                     { get; set; }

        /// <summary>
        /// The homepage of this evse operator.
        /// </summary>
        URL?                            Homepage                         { get; set; }

        /// <summary>
        /// The telephone number of the Charging Station Operator hotline.
        /// </summary>
        PhoneNumber?                    HotlinePhoneNumber               { get; set; }

        #endregion

        #region Events

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data changed.
        /// </summary>
        event OnChargingStationOperatorDataChangedDelegate?         OnDataChanged;

        /// <summary>
        /// An event fired whenever the admin status changed.
        /// </summary>
        event OnChargingStationOperatorAdminStatusChangedDelegate?  OnAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the dynamic status changed.
        /// </summary>
        event OnChargingStationOperatorStatusChangedDelegate?       OnStatusChanged;

        #endregion

        #endregion


        #region Charging pools

        /// <summary>
        /// Called whenever an charging pool will be or was added.
        /// </summary>
        IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingPool, Boolean>                 OnChargingPoolAddition    { get; }

        /// <summary>
        /// Called whenever a charging pool will be or was updated.
        /// </summary>
        IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingPool, IChargingPool, Boolean>  OnChargingPoolUpdate      { get; }

        /// <summary>
        /// Called whenever an charging pool will be or was removed.
        /// </summary>
        IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStationOperator, IChargingPool, Boolean>                 OnChargingPoolRemoval     { get; }



        /// <summary>
        /// Add a new charging pool.
        /// </summary>
        /// <param name="ChargingPool">A new charging pool.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to be called after the successful addition of the charging pool.</param>
        /// <param name="OnError">An optional delegate to be called whenever the addition of the new charging pool failed.</param>
        /// 
        /// <param name="SkipAddedNotifications">Whether to skip sending the 'OnAdded' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        Task<AddChargingPoolResult> AddChargingPool(IChargingPool                                                       ChargingPool,

                                                    Action<IChargingPool,                           EventTracking_Id>?  OnSuccess                      = null,
                                                    Action<IChargingStationOperator, IChargingPool, EventTracking_Id>?  OnError                        = null,

                                                    Boolean                                                             SkipAddedNotifications         = false,
                                                    Func<ChargingStationOperator_Id, ChargingPool_Id, Boolean>?         AllowInconsistentOperatorIds   = null,
                                                    EventTracking_Id?                                                   EventTrackingId                = null,
                                                    User_Id?                                                            CurrentUserId                  = null);


        /// <summary>
        /// Add a new charging pool, but do not fail when this charging pool already exists.
        /// </summary>
        /// <param name="ChargingPool">A new charging pool.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to be called after the successful addition of the charging pool.</param>
        /// 
        /// <param name="SkipAddedNotifications">Whether to skip sending the 'OnAdded' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        Task<AddChargingPoolResult> AddChargingPoolIfNotExists(IChargingPool                                                ChargingPool,

                                                               Action<IChargingPool, EventTracking_Id>?                     OnSuccess                      = null,

                                                               Boolean                                                      SkipAddedNotifications         = false,
                                                               Func<ChargingStationOperator_Id, ChargingPool_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                                               EventTracking_Id?                                            EventTrackingId                = null,
                                                               User_Id?                                                     CurrentUserId                  = null);


        /// <summary>
        /// Add a new or update an existing charging pool.
        /// </summary>
        /// <param name="ChargingPool">A new or updated charging pool.</param>
        /// 
        /// <param name="OnAdditionSuccess">An optional delegate to be called after the successful addition of the charging pool.</param>
        /// <param name="OnUpdateSuccess">An optional delegate to be called after the successful update of the charging pool.</param>
        /// <param name="OnError">An optional delegate to be called whenever the addition of the new charging pool failed.</param>
        /// 
        /// <param name="SkipAddOrUpdatedUpdatedNotifications">Whether to skip sending the 'OnAddedOrUpdated' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        Task<AddOrUpdateChargingPoolResult> AddOrUpdateChargingPool(IChargingPool                                                       ChargingPool,

                                                                    Action<IChargingPool,                           EventTracking_Id>?  OnAdditionSuccess                      = null,
                                                                    Action<IChargingPool,            IChargingPool, EventTracking_Id>?  OnUpdateSuccess                        = null,
                                                                    Action<IChargingStationOperator, IChargingPool, EventTracking_Id>?  OnError                                = null,

                                                                    Boolean                                                             SkipAddOrUpdatedUpdatedNotifications   = false,
                                                                    Func<ChargingStationOperator_Id, ChargingPool_Id, Boolean>?         AllowInconsistentOperatorIds           = null,
                                                                    EventTracking_Id?                                                   EventTrackingId                        = null,
                                                                    User_Id?                                                            CurrentUserId                          = null);


        /// <summary>
        /// Update the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// 
        /// <param name="OnUpdateSuccess">An optional delegate to be called after the successful update of the charging pool.</param>
        /// <param name="OnError">An optional delegate to be called whenever the update of the new charging pool failed.</param>
        /// 
        /// <param name="SkipUpdatedNotifications">Whether to skip sending the 'OnUpdated' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        Task<UpdateChargingPoolResult> UpdateChargingPool(IChargingPool                                                       ChargingPool,

                                                          Action<IChargingPool,            IChargingPool, EventTracking_Id>?  OnUpdateSuccess                = null,
                                                          Action<IChargingStationOperator, IChargingPool, EventTracking_Id>?  OnError                        = null,

                                                          Boolean                                                             SkipUpdatedNotifications       = false,
                                                          Func<ChargingStationOperator_Id, ChargingPool_Id, Boolean>?         AllowInconsistentOperatorIds   = null,
                                                          EventTracking_Id?                                                   EventTrackingId                = null,
                                                          User_Id?                                                            CurrentUserId                  = null);

        /// <summary>
        /// Update the given charging pool.
        /// </summary>
        /// <param name="ChargingPoolId">An unique charging pool identification.</param>
        /// <param name="UpdateDelegate">A delegate for updating the given charging pool.</param>
        /// 
        /// <param name="OnUpdateSuccess">An optional delegate to be called after the successful update of the charging pool.</param>
        /// <param name="OnError">An optional delegate to be called whenever the update of the new charging pool failed.</param>
        /// 
        /// <param name="SkipUpdatedNotifications">Whether to skip sending the 'OnUpdated' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        Task<UpdateChargingPoolResult> UpdateChargingPool(ChargingPool_Id                                                     ChargingPoolId,
                                                          Action<IChargingPool>                                               UpdateDelegate,

                                                          Action<IChargingPool,            IChargingPool, EventTracking_Id>?  OnUpdateSuccess                = null,
                                                          Action<IChargingStationOperator, IChargingPool, EventTracking_Id>?  OnError                        = null,

                                                          Boolean                                                             SkipUpdatedNotifications       = false,
                                                          Func<ChargingStationOperator_Id, ChargingPool_Id, Boolean>?         AllowInconsistentOperatorIds   = null,
                                                          EventTracking_Id?                                                   EventTrackingId                = null,
                                                          User_Id?                                                            CurrentUserId                  = null);


        /// <summary>
        /// Remove the given charging pool.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of the charging pool.</param>
        /// 
        /// <param name="OnRemoveSuccess">An optional delegate to be called after the successful removal of the charging pool.</param>
        /// <param name="OnError">An optional delegate to be called whenever the removal of the new charging pool failed.</param>
        /// 
        /// <param name="SkipRemovedNotifications">Whether to skip sending the 'OnRemoved' event.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        Task<RemoveChargingPoolResult> RemoveChargingPool(ChargingPool_Id                                      ChargingPoolId,

                                                          Action<IChargingPool,            EventTracking_Id>?  OnRemoveSuccess            = null,
                                                          Action<IChargingStationOperator, EventTracking_Id>?  OnError                    = null,

                                                          Boolean                                              SkipRemovedNotifications   = false,
                                                          EventTracking_Id?                                    EventTrackingId            = null,
                                                          User_Id?                                             CurrentUserId              = null);



        /// <summary>
        /// Return an enumeration of all charging pools.
        /// </summary>
        IEnumerable<IChargingPool> ChargingPools { get; }

        /// <summary>
        /// Return an enumeration of all charging pool identifications.
        /// </summary>
        /// <param name="IncludeChargingPools">An optional delegate for filtering charging pools.</param>
        IEnumerable<ChargingPool_Id> ChargingPoolIds(IncludeChargingPoolDelegate? IncludeChargingPools = null);

        /// <summary>
        /// Check if the given ChargingPool is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        Boolean ChargingPoolExists(IChargingPool ChargingPool);

        /// <summary>
        /// Check if the given ChargingPool identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of the charging pool.</param>
        Boolean ChargingPoolExists(ChargingPool_Id ChargingPoolId);

        IChargingPool? GetChargingPoolById(ChargingPool_Id ChargingPoolId);

        Boolean TryGetChargingPoolById(ChargingPool_Id ChargingPoolId, out IChargingPool? ChargingPool);

        Boolean TryGetChargingPoolByStationId(ChargingStation_Id ChargingStationId, out IChargingPool? ChargingPool);








        /// <summary>
        /// Return an enumeration of all charging pool admin status.
        /// </summary>
        /// <param name="IncludeChargingPools">An optional delegate for filtering charging pools.</param>
        IEnumerable<ChargingPoolAdminStatus> ChargingPoolAdminStatus(IncludeChargingPoolDelegate? IncludeChargingPools = null);

        /// <summary>
        /// Return the admin status of all charging pools registered within this roaming network.
        /// </summary>
        /// <param name="IncludeChargingPools">An optional delegate for filtering charging pools.</param>
        /// <param name="TimestampFilter">An optional status timestamp filter.</param>
        /// <param name="AdminStatusFilter">An optional admin status value filter.</param>
        /// <param name="Skip">The number of admin status entries per pool to skip.</param>
        /// <param name="Take">The number of admin status entries per pool to return.</param>
        IEnumerable<Tuple<ChargingPool_Id, IEnumerable<Timestamped<ChargingPoolAdminStatusTypes>>>>

            ChargingPoolAdminStatusSchedule(IncludeChargingPoolDelegate?                  IncludeChargingPools   = null,
                                            Func<DateTime,                     Boolean>?  TimestampFilter        = null,
                                            Func<ChargingPoolAdminStatusTypes, Boolean>?  AdminStatusFilter      = null,
                                            UInt64?                                       Skip                   = null,
                                            UInt64?                                       Take                   = null);

        /// <summary>
        /// Return an enumeration of all charging pool status.
        /// </summary>
        /// <param name="IncludeChargingPools">An optional delegate for filtering charging pools.</param>
        IEnumerable<ChargingPoolStatus> ChargingPoolStatus(IncludeChargingPoolDelegate? IncludeChargingPools = null);

        /// <summary>
        /// Return the admin status of all charging pools registered within this roaming network.
        /// </summary>
        /// <param name="IncludeChargingPools">An optional delegate for filtering charging pools.</param>
        /// <param name="TimestampFilter">An optional status timestamp filter.</param>
        /// <param name="StatusFilter">An optional admin status value filter.</param>
        /// <param name="Skip">The number of status entries per pool to skip.</param>
        /// <param name="Take">The number of status entries per pool to return.</param>
        IEnumerable<Tuple<ChargingPool_Id, IEnumerable<Timestamped<ChargingPoolStatusTypes>>>>

            ChargingPoolStatusSchedule(IncludeChargingPoolDelegate?             IncludeChargingPools   = null,
                                       Func<DateTime,                Boolean>?  TimestampFilter        = null,
                                       Func<ChargingPoolStatusTypes, Boolean>?  StatusFilter           = null,
                                       UInt64?                                  Skip                   = null,
                                       UInt64?                                  Take                   = null);




        void SetChargingPoolAdminStatus(ChargingPool_Id                                         ChargingPoolId,
                                        Timestamped<ChargingPoolAdminStatusTypes>               NewStatus,
                                        Boolean                                                 SendUpstream = false);

        void SetChargingPoolAdminStatus(ChargingPool_Id                                         ChargingPoolId,
                                        ChargingPoolAdminStatusTypes                            NewStatus,
                                        DateTime                                                Timestamp);

        void SetChargingPoolAdminStatus(ChargingPool_Id                                         ChargingPoolId,
                                        IEnumerable<Timestamped<ChargingPoolAdminStatusTypes>>  StatusList,
                                        ChangeMethods                                           ChangeMethod  = ChangeMethods.Replace);


        #region OnChargingPoolData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated charging pool changed.
        /// </summary>
        event OnChargingPoolDataChangedDelegate?         OnChargingPoolDataChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated charging pool changed.
        /// </summary>
        event OnChargingPoolStatusChangedDelegate?       OnChargingPoolStatusChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated charging pool changed.
        /// </summary>
        event OnChargingPoolAdminStatusChangedDelegate?  OnChargingPoolAdminStatusChanged;

        #endregion

        #endregion

        // Charging pool groups

        #region Charging stations

        /// <summary>
        /// Return all charging stations registered within this charing pool.
        /// </summary>
        IEnumerable<IChargingStation> ChargingStations { get; }

        /// <summary>
        /// Return an enumeration of all charging station identifications.
        /// </summary>
        /// <param name="IncludeStations">An optional delegate for filtering charging stations.</param>
        IEnumerable<ChargingStation_Id> ChargingStationIds(IncludeChargingStationDelegate? IncludeStations = null);

        /// <summary>
        /// Return an enumeration of all charging station admin status.
        /// </summary>
        /// <param name="IncludeStations">An optional delegate for filtering charging stations.</param>
        IEnumerable<ChargingStationAdminStatus> ChargingStationAdminStatus(IncludeChargingStationDelegate? IncludeStations = null);

        /// <summary>
        /// Return the admin status of all charging stations registered within this roaming network.
        /// </summary>
        /// <param name="IncludeChargingStations">An optional delegate for filtering charging stations.</param>
        /// <param name="TimestampFilter">An optional status timestamp filter.</param>
        /// <param name="AdminStatusFilter">An optional admin status value filter.</param>
        /// <param name="Skip">The number of admin status entries per station to skip.</param>
        /// <param name="Take">The number of admin status entries per station to return.</param>
        IEnumerable<Tuple<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationAdminStatusTypes>>>>

            ChargingStationAdminStatusSchedule(IncludeChargingStationDelegate?                  IncludeChargingStations   = null,
                                               Func<DateTime,                        Boolean>?  TimestampFilter           = null,
                                               Func<ChargingStationAdminStatusTypes, Boolean>?  AdminStatusFilter         = null,
                                               UInt64?                                          Skip                      = null,
                                               UInt64?                                          Take                      = null);

        /// <summary>
        /// Return an enumeration of all charging station status.
        /// </summary>
        /// <param name="IncludeStations">An optional delegate for filtering charging stations.</param>
        IEnumerable<ChargingStationStatus> ChargingStationStatus(IncludeChargingStationDelegate? IncludeStations = null);

        /// <summary>
        /// Return the admin status of all charging stations registered within this roaming network.
        /// </summary>
        /// <param name="IncludeChargingStations">An optional delegate for filtering charging stations.</param>
        /// <param name="TimestampFilter">An optional status timestamp filter.</param>
        /// <param name="StatusFilter">An optional admin status value filter.</param>
        /// <param name="Skip">The number of status entries per station to skip.</param>
        /// <param name="Take">The number of status entries per station to return.</param>
        IEnumerable<Tuple<ChargingStation_Id, IEnumerable<Timestamped<ChargingStationStatusTypes>>>>

            ChargingStationStatusSchedule(IncludeChargingStationDelegate?             IncludeChargingStations   = null,
                                          Func<DateTime,                   Boolean>?  TimestampFilter           = null,
                                          Func<ChargingStationStatusTypes, Boolean>?  StatusFilter              = null,
                                          UInt64?                                     Skip                      = null,
                                          UInt64?                                     Take                      = null);

        /// <summary>
        /// Called whenever a charging station will be or was added.
        /// </summary>
        IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingPool, IChargingStation, Boolean> OnChargingStationAddition { get; }

        /// <summary>
        /// Called whenever a charging station will be or was updated.
        /// </summary>
        IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingPool, IChargingStation, IChargingStation, Boolean> OnChargingStationUpdate { get; }

        /// <summary>
        /// Called whenever a charging station will be or was removed.
        /// </summary>
        IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingPool, IChargingStation, Boolean> OnChargingStationRemoval { get; }


        /// <summary>
        /// Check if the given ChargingStation is already present within the charging pool.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        Boolean ContainsChargingStation(IChargingStation ChargingStation);

        /// <summary>
        /// Check if the given ChargingStation identification is already present within the charging pool.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the charging station.</param>
        Boolean ContainsChargingStation(ChargingStation_Id ChargingStationId);

        IChargingStation? GetChargingStationById(ChargingStation_Id ChargingStationId);

        Boolean TryGetChargingStationById(ChargingStation_Id ChargingStationId, out IChargingStation? ChargingStation);


        /// <summary>
        /// An event fired whenever the static data of any subordinated charging station changed.
        /// </summary>
        event OnChargingStationDataChangedDelegate?         OnChargingStationDataChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated charging station changed.
        /// </summary>
        event OnChargingStationStatusChangedDelegate?       OnChargingStationStatusChanged;

        /// <summary>
        /// An event fired whenever the aggregated admin status of any subordinated charging station changed.
        /// </summary>
        event OnChargingStationAdminStatusChangedDelegate?  OnChargingStationAdminStatusChanged;


        #region SetChargingStationAdminStatus

        void SetChargingStationAdminStatus(ChargingStation_Id               ChargingStationId,
                                           ChargingStationAdminStatusTypes  NewAdminStatus);

        void SetChargingStationAdminStatus(ChargingStation_Id                            ChargingStationId,
                                           Timestamped<ChargingStationAdminStatusTypes>  NewTimestampedAdminStatus);

        void SetChargingStationAdminStatus(ChargingStation_Id               ChargingStationId,
                                           ChargingStationAdminStatusTypes  NewAdminStatus,
                                           DateTime                         Timestamp);

        void SetChargingStationAdminStatus(ChargingStation_Id                                         ChargingStationId,
                                           IEnumerable<Timestamped<ChargingStationAdminStatusTypes>>  AdminStatusList,
                                           ChangeMethods                                              ChangeMethod  = ChangeMethods.Replace);

        #endregion

        #region SetChargingStationStatus

        void SetChargingStationStatus(ChargingStation_Id          ChargingStationId,
                                      ChargingStationStatusTypes  NewStatus);

        void SetChargingStationStatus(ChargingStation_Id                       ChargingStationId,
                                      Timestamped<ChargingStationStatusTypes>  NewTimestampedStatus);

        void SetChargingStationStatus(ChargingStation_Id          ChargingStationId,
                                      ChargingStationStatusTypes  NewStatus,
                                      DateTime                    Timestamp);

        void SetChargingStationStatus(ChargingStation_Id                                    ChargingStationId,
                                      IEnumerable<Timestamped<ChargingStationStatusTypes>>  StatusList,
                                      ChangeMethods                                         ChangeMethod  = ChangeMethods.Replace);

        #endregion

        #endregion

        #region Charging station groups

        /// <summary>
        /// Called whenever a charging station group will be or was added.
        /// </summary>
        IVotingSender<DateTime, ChargingStationOperator, ChargingStationGroup, Boolean> OnChargingStationGroupAddition { get; }

        /// <summary>
        /// All charging station groups registered within this charging station operator.
        /// </summary>
        IEnumerable<ChargingStationGroup> ChargingStationGroups { get; }


        /// <summary>
        /// Create and register a new charging group having the given
        /// unique charging group identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing station group.</param>
        /// <param name="Name">The offical (multi-language) name of this charging station group.</param>
        /// <param name="Description">An optional (multi-language) description of this charging station group.</param>
        /// 
        /// <param name="Members">An enumeration of charging stations member building this charging station group.</param>
        /// <param name="MemberIds">An enumeration of charging station identifications which are building this charging station group.</param>
        /// <param name="AutoIncludeStations">A delegate deciding whether to include new charging stations automatically into this group.</param>
        /// 
        /// <param name="StatusAggregationDelegate">A delegate called to aggregate the dynamic status of all subordinated charging stations.</param>
        /// <param name="MaxGroupStatusListSize">The default size of the charging station group status list.</param>
        /// <param name="MaxGroupAdminStatusListSize">The default size of the charging station group admin status list.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging group after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging group failed.</param>
        ChargingStationGroup CreateChargingStationGroup(ChargingStationGroup_Id                                             Id,
                                                        I18NString                                                          Name,
                                                        I18NString                                                          Description                   = null,

                                                        Brand                                                               Brand                         = null,
                                                        Priority?                                                           Priority                      = null,
                                                        ChargingTariff                                                      Tariff                        = null,
                                                        IEnumerable<OpenDataLicense>                                            DataLicenses                  = null,

                                                        IEnumerable<IChargingStation>                                       Members                       = null,
                                                        IEnumerable<ChargingStation_Id>                                     MemberIds                     = null,
                                                        Func<IChargingStation, Boolean>                                     AutoIncludeStations           = null,

                                                        Func<ChargingStationStatusReport, ChargingStationGroupStatusTypes>  StatusAggregationDelegate     = null,
                                                        UInt16                                                              MaxGroupStatusListSize        = ChargingStationGroup.DefaultMaxGroupStatusListSize,
                                                        UInt16                                                              MaxGroupAdminStatusListSize   = ChargingStationGroup.DefaultMaxGroupAdminStatusListSize,

                                                        Action<ChargingStationGroup>                                        OnSuccess                     = null,
                                                        Action<IChargingStationOperator, ChargingStationGroup_Id>           OnError                       = null);

        /// <summary>
        /// Create and register a new charging group having the given
        /// unique charging group identification.
        /// </summary>
        /// <param name="IdSuffix">The suffix of the unique identification of the new charging group.</param>
        /// <param name="Name">The offical (multi-language) name of this charging station group.</param>
        /// <param name="Description">An optional (multi-language) description of this charging station group.</param>
        /// 
        /// <param name="Members">An enumeration of charging stations member building this charging station group.</param>
        /// <param name="MemberIds">An enumeration of charging station identifications which are building this charging station group.</param>
        /// <param name="AutoIncludeStations">A delegate deciding whether to include new charging stations automatically into this group.</param>
        /// 
        /// <param name="StatusAggregationDelegate">A delegate called to aggregate the dynamic status of all subordinated charging stations.</param>
        /// <param name="MaxGroupStatusListSize">The default size of the charging station group status list.</param>
        /// <param name="MaxGroupAdminStatusListSize">The default size of the charging station group admin status list.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging group after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging group failed.</param>
        ChargingStationGroup CreateChargingStationGroup(String                                                              IdSuffix,
                                                        I18NString                                                          Name,
                                                        I18NString                                                          Description                   = null,

                                                        IEnumerable<IChargingStation>                                       Members                       = null,
                                                        IEnumerable<ChargingStation_Id>                                     MemberIds                     = null,
                                                        Func<IChargingStation, Boolean>                                     AutoIncludeStations           = null,

                                                        Func<ChargingStationStatusReport, ChargingStationGroupStatusTypes>  StatusAggregationDelegate     = null,
                                                        UInt16                                                              MaxGroupStatusListSize        = ChargingStationGroup.DefaultMaxGroupStatusListSize,
                                                        UInt16                                                              MaxGroupAdminStatusListSize   = ChargingStationGroup.DefaultMaxGroupAdminStatusListSize,

                                                        Action<ChargingStationGroup>                                        OnSuccess                     = null,
                                                        Action<IChargingStationOperator, ChargingStationGroup_Id>           OnError                       = null);

        /// <summary>
        /// Get or create and register a new charging group having the given
        /// unique charging group identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing station group.</param>
        /// <param name="Name">The offical (multi-language) name of this charging station group.</param>
        /// <param name="Description">An optional (multi-language) description of this charging station group.</param>
        /// 
        /// <param name="Members">An enumeration of charging stations member building this charging station group.</param>
        /// <param name="MemberIds">An enumeration of charging station identifications which are building this charging station group.</param>
        /// <param name="AutoIncludeStations">A delegate deciding whether to include new charging stations automatically into this group.</param>
        /// 
        /// <param name="StatusAggregationDelegate">A delegate called to aggregate the dynamic status of all subordinated charging stations.</param>
        /// <param name="MaxGroupStatusListSize">The default size of the charging station group status list.</param>
        /// <param name="MaxGroupAdminStatusListSize">The default size of the charging station group admin status list.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging group after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging group failed.</param>
        ChargingStationGroup GetOrCreateChargingStationGroup(ChargingStationGroup_Id                                             Id,
                                                             I18NString                                                          Name,
                                                             I18NString                                                          Description                   = null,

                                                             IEnumerable<IChargingStation>                                       Members                       = null,
                                                             IEnumerable<ChargingStation_Id>                                     MemberIds                     = null,
                                                             Func<IChargingStation, Boolean>                                     AutoIncludeStations           = null,

                                                             Func<ChargingStationStatusReport, ChargingStationGroupStatusTypes>  StatusAggregationDelegate     = null,
                                                             UInt16                                                              MaxGroupStatusListSize        = ChargingStationGroup.DefaultMaxGroupStatusListSize,
                                                             UInt16                                                              MaxGroupAdminStatusListSize   = ChargingStationGroup.DefaultMaxGroupAdminStatusListSize,

                                                             Action<ChargingStationGroup>                                        OnSuccess                     = null,
                                                             Action<IChargingStationOperator, ChargingStationGroup_Id>           OnError                       = null);

        /// <summary>
        /// Get or create and register a new charging group having the given
        /// unique charging group identification.
        /// </summary>
        /// <param name="IdSuffix">The suffix of the unique identification of the new charging group.</param>
        /// <param name="Name">The offical (multi-language) name of this charging station group.</param>
        /// <param name="Description">An optional (multi-language) description of this charging station group.</param>
        /// 
        /// <param name="Members">An enumeration of charging stations member building this charging station group.</param>
        /// <param name="MemberIds">An enumeration of charging station identifications which are building this charging station group.</param>
        /// <param name="AutoIncludeStations">A delegate deciding whether to include new charging stations automatically into this group.</param>
        /// 
        /// <param name="StatusAggregationDelegate">A delegate called to aggregate the dynamic status of all subordinated charging stations.</param>
        /// <param name="MaxGroupStatusListSize">The default size of the charging station group status list.</param>
        /// <param name="MaxGroupAdminStatusListSize">The default size of the charging station group admin status list.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging group after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging group failed.</param>
        ChargingStationGroup GetOrCreateChargingStationGroup(String                                                              IdSuffix,
                                                             I18NString                                                          Name,
                                                             I18NString                                                          Description                   = null,

                                                             IEnumerable<IChargingStation>                                       Members                       = null,
                                                             IEnumerable<ChargingStation_Id>                                     MemberIds                     = null,
                                                             Func<IChargingStation, Boolean>                                     AutoIncludeStations           = null,

                                                             Func<ChargingStationStatusReport, ChargingStationGroupStatusTypes>  StatusAggregationDelegate     = null,
                                                             UInt16                                                              MaxGroupStatusListSize        = ChargingStationGroup.DefaultMaxGroupStatusListSize,
                                                             UInt16                                                              MaxGroupAdminStatusListSize   = ChargingStationGroup.DefaultMaxGroupAdminStatusListSize,

                                                             Action<ChargingStationGroup>                                        OnSuccess                     = null,
                                                             Action<IChargingStationOperator, ChargingStationGroup_Id>           OnError                       = null);

        /// <summary>
        /// Try to return to charging station group for the given charging station group identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing station group.</param>
        /// <param name="ChargingStationGroup">The charing station group.</param>
        Boolean TryGetChargingStationGroup(ChargingStationGroup_Id   Id,
                                           out ChargingStationGroup  ChargingStationGroup);

        /// <summary>
        /// Called whenever a charging station group will be or was removed.
        /// </summary>
        IVotingSender<DateTime, ChargingStationOperator, ChargingStationGroup, Boolean> OnChargingStationGroupRemoval { get; }


        /// <summary>
        /// All charging station groups registered within this charging station operator.
        /// </summary>
        /// <param name="ChargingStationGroupId">The unique identification of the charging station group to be removed.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging station group after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the charging station group failed.</param>
        ChargingStationGroup RemoveChargingStationGroup(ChargingStationGroup_Id                                   ChargingStationGroupId,
                                                        Action<ChargingStationOperator, ChargingStationGroup>     OnSuccess   = null,
                                                        Action<ChargingStationOperator, ChargingStationGroup_Id>  OnError     = null);

        /// <summary>
        /// All charging station groups registered within this charging station operator.
        /// </summary>
        /// <param name="ChargingStationGroup">The charging station group to remove.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging station group after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the charging station group failed.</param>
        ChargingStationGroup RemoveChargingStationGroup(ChargingStationGroup                                   ChargingStationGroup,
                                                        Action<ChargingStationOperator, ChargingStationGroup>  OnSuccess   = null,
                                                        Action<ChargingStationOperator, ChargingStationGroup>  OnError     = null);

        #endregion

        #region EVSEs

        IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean> EVSEAddition { get; }

        /// <summary>
        /// Called whenever an EVSE will be or was added.
        /// </summary>
        IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean> OnEVSEAddition { get; }



        IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, IEVSE, Boolean> EVSEUpdate { get; }

        /// <summary>
        /// Called whenever an EVSE will be or was updated.
        /// </summary>
        IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, IEVSE, Boolean> OnEVSEUpdate { get; }



        IVotingNotificator<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean> EVSERemoval { get; }

        /// <summary>
        /// Called whenever an EVSE will be or was removed.
        /// </summary>
        IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean> OnEVSERemoval { get; }



        /// <summary>
        /// All Electric Vehicle Supply Equipments (EVSE) present
        /// within this charging pool.
        /// </summary>
        IEnumerable<IEVSE> EVSEs { get; }

        /// <summary>
        /// The unique identifications of all Electric Vehicle Supply Equipment
        /// (EVSEs) present within this charging pool.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        IEnumerable<EVSE_Id> EVSEIds(IncludeEVSEDelegate? IncludeEVSEs = null);

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        IEnumerable<EVSEAdminStatus> EVSEAdminStatus(IncludeEVSEDelegate? IncludeEVSEs = null);

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        IEnumerable<Tuple<EVSE_Id, IEnumerable<Timestamped<EVSEAdminStatusTypes>>>>

            EVSEAdminStatusSchedule(IncludeEVSEDelegate?                  IncludeEVSEs      = null,
                                    Func<DateTime,             Boolean>?  TimestampFilter   = null,
                                    Func<EVSEAdminStatusTypes, Boolean>?  StatusFilter      = null,
                                    UInt64?                               Skip              = null,
                                    UInt64?                               Take              = null);

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        IEnumerable<EVSEStatus> EVSEStatus(IncludeEVSEDelegate? IncludeEVSEs = null);

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        IEnumerable<Tuple<EVSE_Id, IEnumerable<Timestamped<EVSEStatusTypes>>>>

            EVSEStatusSchedule(IncludeEVSEDelegate?             IncludeEVSEs      = null,
                               Func<DateTime,        Boolean>?  TimestampFilter   = null,
                               Func<EVSEStatusTypes, Boolean>?  StatusFilter      = null,
                               UInt64?                          Skip              = null,
                               UInt64?                          Take              = null);



        /// <summary>
        /// Check if the given EVSE is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        Boolean ContainsEVSE(EVSE EVSE);

        /// <summary>
        /// Check if the given EVSE identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        Boolean ContainsEVSE(EVSE_Id EVSEId);

        IEVSE? GetEVSEById(EVSE_Id EVSEId);

        Boolean TryGetEVSEById(EVSE_Id EVSEId, out IEVSE? EVSE);

        Boolean TryGetChargingStationByEVSEId(EVSE_Id  EVSEId, out IChargingStation? ChargingStation);
        Boolean TryGetChargingStationByEVSEId(EVSE_Id? EVSEId, out IChargingStation? ChargingStation);

        Boolean TryGetChargingPoolByEVSEId(EVSE_Id  EVSEId, out IChargingPool? ChargingPool);
        Boolean TryGetChargingPoolByEVSEId(EVSE_Id? EVSEId, out IChargingPool? ChargingPool);


        #region OnEVSEData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE changed.
        /// </summary>
        event OnEVSEDataChangedDelegate         OnEVSEDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        event OnEVSEStatusChangedDelegate       OnEVSEStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        event OnEVSEAdminStatusChangedDelegate  OnEVSEAdminStatusChanged;

        #endregion

        //#region SocketOutletAddition

        //internal readonly IVotingNotificator<DateTime, EVSE, SocketOutlet, Boolean> SocketOutletAddition;

        ///// <summary>
        ///// Called whenever a socket outlet will be or was added.
        ///// </summary>
        //public IVotingSender<DateTime, EVSE, SocketOutlet, Boolean> OnSocketOutletAddition

        //    => SocketOutletAddition;

        //#endregion

        //#region SocketOutletRemoval

        //internal readonly IVotingNotificator<DateTime, EVSE, SocketOutlet, Boolean> SocketOutletRemoval;

        ///// <summary>
        ///// Called whenever a socket outlet will be or was removed.
        ///// </summary>
        //public IVotingSender<DateTime, EVSE, SocketOutlet, Boolean> OnSocketOutletRemoval

        //    => SocketOutletRemoval;

        //#endregion


        #region SetEVSEAdminStatus
        void SetEVSEAdminStatus(EVSEAdminStatus NewAdminStatus);

        void SetEVSEAdminStatus(EVSE_Id EVSEId,
                                EVSEAdminStatusTypes NewAdminStatus);

        void SetEVSEAdminStatus(EVSE_Id                            EVSEId,
                                Timestamped<EVSEAdminStatusTypes>  NewTimestampedAdminStatus);

        void SetEVSEAdminStatus(EVSE_Id               EVSEId,
                                EVSEAdminStatusTypes  NewAdminStatus,
                                DateTime              Timestamp);

        void SetEVSEAdminStatus(EVSE_Id                                         EVSEId,
                                IEnumerable<Timestamped<EVSEAdminStatusTypes>>  AdminStatusList,
                                ChangeMethods                                   ChangeMethod  = ChangeMethods.Replace);

        EVSEAdminStatusDiff ApplyEVSEAdminStatusDiff(EVSEAdminStatusDiff EVSEAdminStatusDiff);

        #endregion

        #region SetEVSEStatus

        void SetEVSEStatus(EVSEStatus  NewStatus);

        void SetEVSEStatus(EVSE_Id          EVSEId,
                           EVSEStatusTypes  NewStatus);

        void SetEVSEStatus(EVSE_Id                       EVSEId,
                           Timestamped<EVSEStatusTypes>  NewTimestampedStatus);

        void SetEVSEStatus(EVSE_Id          EVSEId,
                           EVSEStatusTypes  NewStatus,
                           DateTime         Timestamp);

        void SetEVSEStatus(EVSE_Id                                    EVSEId,
                           IEnumerable<Timestamped<EVSEStatusTypes>>  StatusList,
                           ChangeMethods                              ChangeMethod  = ChangeMethods.Replace);

        EVSEStatusDiff ApplyEVSEStatusDiff(EVSEStatusDiff EVSEStatusDiff);

        #endregion

        #endregion

        #region EVSE groups

        /// <summary>
        /// Called whenever an EVSE group will be or was added.
        /// </summary>
        IVotingSender<DateTime, ChargingStationOperator, EVSEGroup, Boolean> OnEVSEGroupAddition { get; }

        /// <summary>
        /// All EVSE groups registered within this charging station operator.
        /// </summary>
        IEnumerable<EVSEGroup> EVSEGroups        { get; }


        /// <summary>
        /// Create and register a new charging group having the given
        /// unique charging group identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing station group.</param>
        /// <param name="Name">The offical (multi-language) name of this EVSE group.</param>
        /// <param name="Description">An optional (multi-language) description of this EVSE group.</param>
        /// 
        /// <param name="Members">An enumeration of EVSEs member building this EVSE group.</param>
        /// <param name="MemberIds">An enumeration of EVSE identifications which are building this EVSE group.</param>
        /// <param name="AutoIncludeStations">A delegate deciding whether to include new EVSEs automatically into this group.</param>
        /// 
        /// <param name="StatusAggregationDelegate">A delegate called to aggregate the dynamic status of all subordinated EVSEs.</param>
        /// <param name="MaxGroupStatusListSize">The default size of the EVSE group status list.</param>
        /// <param name="MaxGroupAdminStatusListSize">The default size of the EVSE group admin status list.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging group after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging group failed.</param>
        EVSEGroup CreateEVSEGroup(EVSEGroup_Id                                   Id,
                                  I18NString                                     Name,
                                  I18NString                                     Description                   = null,

                                  Brand                                          Brand                         = null,
                                  Priority?                                      Priority                      = null,
                                  ChargingTariff                                 Tariff                        = null,
                                  IEnumerable<OpenDataLicense>                       DataLicenses                  = null,

                                  IEnumerable<EVSE>                              Members                       = null,
                                  IEnumerable<EVSE_Id>                           MemberIds                     = null,
                                  Func<EVSE_Id, Boolean>                         AutoIncludeEVSEIds            = null,
                                  Func<IEVSE,   Boolean>                         AutoIncludeEVSEs              = null,

                                  Func<EVSEStatusReport, EVSEGroupStatusTypes>   StatusAggregationDelegate     = null,
                                  UInt16                                         MaxGroupStatusListSize        = EVSEGroup.DefaultMaxGroupStatusListSize,
                                  UInt16                                         MaxGroupAdminStatusListSize   = EVSEGroup.DefaultMaxGroupAdminStatusListSize,

                                  Action<EVSEGroup>                              OnSuccess                     = null,
                                  Action<ChargingStationOperator, EVSEGroup_Id>  OnError                       = null);

        /// <summary>
        /// Create and register a new charging group having the given
        /// unique charging group identification.
        /// </summary>
        /// <param name="IdSuffix">The suffix of the unique identification of the new charging group.</param>
        /// <param name="Name">The offical (multi-language) name of this EVSE group.</param>
        /// <param name="Description">An optional (multi-language) description of this EVSE group.</param>
        /// 
        /// <param name="Members">An enumeration of EVSEs member building this EVSE group.</param>
        /// <param name="MemberIds">An enumeration of EVSE identifications which are building this EVSE group.</param>
        /// <param name="AutoIncludeStations">A delegate deciding whether to include new EVSEs automatically into this group.</param>
        /// 
        /// <param name="StatusAggregationDelegate">A delegate called to aggregate the dynamic status of all subordinated EVSEs.</param>
        /// <param name="MaxGroupStatusListSize">The default size of the EVSE group status list.</param>
        /// <param name="MaxGroupAdminStatusListSize">The default size of the EVSE group admin status list.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging group after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging group failed.</param>
        EVSEGroup CreateEVSEGroup(String                                         IdSuffix,
                                  I18NString                                     Name,
                                  I18NString                                     Description                   = null,

                                  Brand                                          Brand                         = null,
                                  Priority?                                      Priority                      = null,
                                  ChargingTariff                                 Tariff                        = null,
                                  IEnumerable<OpenDataLicense>                       DataLicenses                  = null,

                                  IEnumerable<EVSE>                              Members                       = null,
                                  IEnumerable<EVSE_Id>                           MemberIds                     = null,
                                  Func<EVSE_Id, Boolean>                         AutoIncludeEVSEIds            = null,
                                  Func<IEVSE,   Boolean>                         AutoIncludeEVSEs              = null,

                                  Func<EVSEStatusReport, EVSEGroupStatusTypes>   StatusAggregationDelegate     = null,
                                  UInt16                                         MaxGroupStatusListSize        = EVSEGroup.DefaultMaxGroupStatusListSize,
                                  UInt16                                         MaxGroupAdminStatusListSize   = EVSEGroup.DefaultMaxGroupAdminStatusListSize,

                                  Action<EVSEGroup>                              OnSuccess                     = null,
                                  Action<ChargingStationOperator, EVSEGroup_Id>  OnError                       = null);

        /// <summary>
        /// Get or create and register a new charging group having the given
        /// unique charging group identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing station group.</param>
        /// <param name="Name">The offical (multi-language) name of this EVSE group.</param>
        /// <param name="Description">An optional (multi-language) description of this EVSE group.</param>
        /// 
        /// <param name="Members">An enumeration of EVSEs member building this EVSE group.</param>
        /// <param name="MemberIds">An enumeration of EVSE identifications which are building this EVSE group.</param>
        /// <param name="AutoIncludeStations">A delegate deciding whether to include new EVSEs automatically into this group.</param>
        /// 
        /// <param name="StatusAggregationDelegate">A delegate called to aggregate the dynamic status of all subordinated EVSEs.</param>
        /// <param name="MaxGroupStatusListSize">The default size of the EVSE group status list.</param>
        /// <param name="MaxGroupAdminStatusListSize">The default size of the EVSE group admin status list.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging group after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging group failed.</param>
        EVSEGroup GetOrCreateEVSEGroup(EVSEGroup_Id                                   Id,
                                       I18NString                                     Name,
                                       I18NString                                     Description                   = null,

                                       Brand                                          Brand                         = null,
                                       Priority?                                      Priority                      = null,
                                       ChargingTariff                                 Tariff                        = null,
                                       IEnumerable<OpenDataLicense>                       DataLicenses                  = null,

                                       IEnumerable<EVSE>                              Members                       = null,
                                       IEnumerable<EVSE_Id>                           MemberIds                     = null,
                                       Func<EVSE_Id, Boolean>                         AutoIncludeEVSEIds            = null,
                                       Func<IEVSE,   Boolean>                         AutoIncludeEVSEs              = null,

                                       Func<EVSEStatusReport, EVSEGroupStatusTypes>   StatusAggregationDelegate     = null,
                                       UInt16                                         MaxGroupStatusListSize        = EVSEGroup.DefaultMaxGroupStatusListSize,
                                       UInt16                                         MaxGroupAdminStatusListSize   = EVSEGroup.DefaultMaxGroupAdminStatusListSize,

                                       Action<EVSEGroup>                              OnSuccess                     = null,
                                       Action<ChargingStationOperator, EVSEGroup_Id>  OnError                       = null);

        /// <summary>
        /// Get or create and register a new charging group having the given
        /// unique charging group identification.
        /// </summary>
        /// <param name="IdSuffix">The suffix of the unique identification of the new charging group.</param>
        /// <param name="Name">The offical (multi-language) name of this EVSE group.</param>
        /// <param name="Description">An optional (multi-language) description of this EVSE group.</param>
        /// 
        /// <param name="Members">An enumeration of EVSEs member building this EVSE group.</param>
        /// <param name="MemberIds">An enumeration of EVSE identifications which are building this EVSE group.</param>
        /// <param name="AutoIncludeStations">A delegate deciding whether to include new EVSEs automatically into this group.</param>
        /// 
        /// <param name="StatusAggregationDelegate">A delegate called to aggregate the dynamic status of all subordinated EVSEs.</param>
        /// <param name="MaxGroupStatusListSize">The default size of the EVSE group status list.</param>
        /// <param name="MaxGroupAdminStatusListSize">The default size of the EVSE group admin status list.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging group after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging group failed.</param>
        EVSEGroup GetOrCreateEVSEGroup(String                                         IdSuffix,
                                       I18NString                                     Name,
                                       I18NString                                     Description                   = null,

                                       Brand                                          Brand                         = null,
                                       Priority?                                      Priority                      = null,
                                       ChargingTariff                                 Tariff                        = null,
                                       IEnumerable<OpenDataLicense>                       DataLicenses                  = null,

                                       IEnumerable<EVSE>                              Members                       = null,
                                       IEnumerable<EVSE_Id>                           MemberIds                     = null,
                                       Func<EVSE_Id, Boolean>                         AutoIncludeEVSEIds            = null,
                                       Func<IEVSE,   Boolean>                         AutoIncludeEVSEs              = null,

                                       Func<EVSEStatusReport, EVSEGroupStatusTypes>   StatusAggregationDelegate     = null,
                                       UInt16                                         MaxGroupStatusListSize        = EVSEGroup.DefaultMaxGroupStatusListSize,
                                       UInt16                                         MaxGroupAdminStatusListSize   = EVSEGroup.DefaultMaxGroupAdminStatusListSize,

                                       Action<EVSEGroup>                              OnSuccess                     = null,
                                       Action<ChargingStationOperator, EVSEGroup_Id>  OnError                       = null);



        /// <summary>
        /// Try to return to EVSE group for the given EVSE group identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing station group.</param>
        /// <param name="EVSEGroup">The charing station group.</param>
        Boolean TryGetEVSEGroup(EVSEGroup_Id    Id,
                                out EVSEGroup?  EVSEGroup);


        /// <summary>
        /// Called whenever an EVSE group will be or was removed.
        /// </summary>
        IVotingSender<DateTime, ChargingStationOperator, EVSEGroup, Boolean> OnEVSEGroupRemoval { get; }



        /// <summary>
        /// All EVSE groups registered within this charging station operator.
        /// </summary>
        /// <param name="EVSEGroupId">The unique identification of the EVSE group to be removed.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new EVSE group after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the EVSE group failed.</param>
        EVSEGroup RemoveEVSEGroup(EVSEGroup_Id                                   EVSEGroupId,
                                  Action<ChargingStationOperator, EVSEGroup>     OnSuccess   = null,
                                  Action<ChargingStationOperator, EVSEGroup_Id>  OnError     = null);

        /// <summary>
        /// All EVSE groups registered within this charging station operator.
        /// </summary>
        /// <param name="EVSEGroup">The EVSE group to remove.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new EVSE group after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the EVSE group failed.</param>
        EVSEGroup RemoveEVSEGroup(EVSEGroup                                   EVSEGroup,
                                  Action<ChargingStationOperator, EVSEGroup>  OnSuccess   = null,
                                  Action<ChargingStationOperator, EVSEGroup>  OnError     = null);

        #endregion



        #region ChargingTariffs

        IVotingNotificator<DateTime, ChargingStationOperator, ChargingTariff, Boolean> ChargingTariffAddition { get; }

        /// <summary>
        /// Called whenever a charging tariff will be or was added.
        /// </summary>
        IVotingSender<DateTime, ChargingStationOperator, ChargingTariff, Boolean> OnChargingTariffAddition { get; }


        /// <summary>
        /// All charging tariffs registered within this charging station operator.
        /// </summary>
        IEnumerable<ChargingTariff> ChargingTariffs { get; }


        URL? TermsAndConditionsURL { get; }


        /// <summary>
        /// Create and register a new charging tariff having the given
        /// unique charging tariff identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing tariff.</param>
        /// <param name="Name">The offical (multi-language) name of this charging tariff.</param>
        /// <param name="Description">An optional (multi-language) description of this charging tariff.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging tariff after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging tariff failed.</param>
        ChargingTariff? CreateChargingTariff(ChargingTariff_Id                                    Id,
                                             I18NString                                           Name,
                                             I18NString                                           Description,
                                             IEnumerable<ChargingTariffElement>                   TariffElements,
                                             Currency                                             Currency,
                                             Brand                                                Brand,
                                             URL                                                  TariffURL,
                                             EnergyMix                                            EnergyMix,

                                             Action<ChargingTariff>?                              OnSuccess   = null,
                                             Action<ChargingStationOperator, ChargingTariff_Id>?  OnError     = null);

        /// <summary>
        /// Create and register a new charging tariff having the given
        /// unique charging tariff identification.
        /// </summary>
        /// <param name="IdSuffix">The suffix of the unique identification of the new charging tariff.</param>
        /// <param name="Name">The offical (multi-language) name of this charging tariff.</param>
        /// <param name="Description">An optional (multi-language) description of this charging tariff.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging tariff after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging tariff failed.</param>
        ChargingTariff? CreateChargingTariff(String                                               IdSuffix,
                                             I18NString                                           Name,
                                             I18NString                                           Description,
                                             IEnumerable<ChargingTariffElement>                   TariffElements,
                                             Currency                                             Currency,
                                             Brand                                                Brand,
                                             URL                                                  TariffURL,
                                             EnergyMix                                            EnergyMix,

                                             Action<ChargingTariff>?                              OnSuccess   = null,
                                             Action<ChargingStationOperator, ChargingTariff_Id>?  OnError     = null);

        /// <summary>
        /// Get or create and register a new charging tariff having the given
        /// unique charging tariff identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing tariff.</param>
        /// <param name="Name">The offical (multi-language) name of this charging tariff.</param>
        /// <param name="Description">An optional (multi-language) description of this charging tariff.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging tariff after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging tariff failed.</param>
        ChargingTariff? GetOrCreateChargingTariff(ChargingTariff_Id                                    Id,
                                                  I18NString                                           Name,
                                                  I18NString                                           Description,
                                                  IEnumerable<ChargingTariffElement>                   TariffElements,
                                                  Currency                                             Currency,
                                                  Brand                                                Brand,
                                                  URL                                                  TariffURL,
                                                  EnergyMix                                            EnergyMix,

                                                  Action<ChargingTariff>?                              OnSuccess   = null,
                                                  Action<ChargingStationOperator, ChargingTariff_Id>?  OnError     = null);

        /// <summary>
        /// Get or create and register a new charging tariff having the given
        /// unique charging tariff identification.
        /// </summary>
        /// <param name="IdSuffix">The suffix of the unique identification of the new charging tariff.</param>
        /// <param name="Name">The offical (multi-language) name of this charging tariff.</param>
        /// <param name="Description">An optional (multi-language) description of this charging tariff.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to configure the new charging tariff after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging tariff failed.</param>
        ChargingTariff? GetOrCreateChargingTariff(String                                               IdSuffix,
                                                  I18NString                                           Name,
                                                  I18NString                                           Description,
                                                  IEnumerable<ChargingTariffElement>                   TariffElements,
                                                  Currency                                             Currency,
                                                  Brand                                                Brand,
                                                  URL                                                  TariffURL,
                                                  EnergyMix                                            EnergyMix,

                                                  Action<ChargingTariff>?                              OnSuccess   = null,
                                                  Action<ChargingStationOperator, ChargingTariff_Id>?  OnError     = null);

        /// <summary>
        /// Return to charging tariff for the given charging tariff identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing tariff.</param>
        ChargingTariff? GetChargingTariff(ChargingTariff_Id Id);

        /// <summary>
        /// Try to return to charging tariff for the given charging tariff identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing tariff.</param>
        /// <param name="ChargingTariff">The charing tariff.</param>
        Boolean TryGetChargingTariff(ChargingTariff_Id    Id,
                                     out ChargingTariff?  ChargingTariff);

        IEnumerable<ChargingTariff>     GetChargingTariffs  (ChargingPool_Id?       ChargingPoolId        = null,
                                                             ChargingStation_Id?    ChargingStationId     = null,
                                                             EVSE_Id?               EVSEId                = null,
                                                             ChargingConnector_Id?  ChargingConnectorId   = null,
                                                             EMobilityProvider_Id?  EMobilityProviderId   = null);

        IEnumerable<ChargingTariff_Id>  GetChargingTariffIds(ChargingPool_Id?       ChargingPoolId        = null,
                                                             ChargingStation_Id?    ChargingStationId     = null,
                                                             EVSE_Id?               EVSEId                = null,
                                                             ChargingConnector_Id?  ChargingConnectorId   = null,
                                                             EMobilityProvider_Id?  EMobilityProviderId   = null);


        IVotingNotificator<DateTime, ChargingStationOperator, ChargingTariff, Boolean> ChargingTariffRemoval { get; }

        /// <summary>
        /// Called whenever a charging tariff will be or was removed.
        /// </summary>
        IVotingSender<DateTime, ChargingStationOperator, ChargingTariff, Boolean> OnChargingTariffRemoval { get; }


        /// <summary>
        /// All charging tariffs registered within this charging station operator.
        /// </summary>
        /// <param name="ChargingTariffId">The unique identification of the charging tariff to be removed.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging tariff after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the charging tariff failed.</param>
        ChargingTariff? RemoveChargingTariff(ChargingTariff_Id                                     ChargingTariffId,
                                             Action<IChargingStationOperator, ChargingTariff>?     OnSuccess   = null,
                                             Action<IChargingStationOperator, ChargingTariff_Id>?  OnError     = null);

        /// <summary>
        /// All charging tariffs registered within this charging station operator.
        /// </summary>
        /// <param name="ChargingTariff">The charging tariff to remove.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging tariff after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the charging tariff failed.</param>
        ChargingTariff? RemoveChargingTariff(ChargingTariff                                     ChargingTariff,
                                             Action<IChargingStationOperator, ChargingTariff>?  OnSuccess   = null,
                                             Action<IChargingStationOperator, ChargingTariff>?  OnError     = null);

        #endregion

        #region ChargingTariffGroups

        IVotingNotificator<DateTime, ChargingStationOperator, ChargingTariffGroup, Boolean> ChargingTariffGroupAddition { get; }

        /// <summary>
        /// Called whenever a charging tariff will be or was added.
        /// </summary>
        IVotingSender<DateTime, ChargingStationOperator, ChargingTariffGroup, Boolean> OnChargingTariffGroupAddition { get; }

        /// <summary>
        /// All charging tariff groups registered within this charging station operator.
        /// </summary>
        IEnumerable<ChargingTariffGroup> ChargingTariffGroups { get; }

        /// <summary>
        /// Create and register a new charging tariff group having the given
        /// unique charging tariff identification.
        /// </summary>
        /// <param name="IdSuffix">The suffix of the unique identification of the charing tariff group.</param>
        /// <param name="Description">An optional (multi-language) description of this charging tariff group.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging tariff group after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging tariff group failed.</param>
        ChargingTariffGroup? CreateChargingTariffGroup(String                                                     IdSuffix,
                                                       I18NString                                                 Description,
                                                       Action<ChargingTariffGroup>?                               OnSuccess   = null,
                                                       Action<IChargingStationOperator, ChargingTariffGroup_Id>?  OnError     = null);

        /// <summary>
        /// Get or create and register a new charging tariff having the given
        /// unique charging tariff identification.
        /// </summary>
        /// <param name="IdSuffix">The suffix of the unique identification of the charing tariff group.</param>
        /// <param name="Description">An optional (multi-language) description of this charging tariff.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging tariff after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging tariff failed.</param>
        ChargingTariffGroup? GetOrCreateChargingTariffGroup(String                                                     IdSuffix,
                                                            I18NString                                                 Description,
                                                            Action<ChargingTariffGroup>?                               OnSuccess   = null,
                                                            Action<IChargingStationOperator, ChargingTariffGroup_Id>?  OnError     = null);

        /// <summary>
        /// Return to charging tariff for the given charging tariff identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing tariff.</param>
        ChargingTariffGroup? GetChargingTariffGroup(ChargingTariffGroup_Id Id);

        /// <summary>
        /// Try to return to charging tariff for the given charging tariff identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing tariff.</param>
        /// <param name="ChargingTariffGroup">The charing tariff.</param>
        Boolean TryGetChargingTariffGroup(ChargingTariffGroup_Id    Id,
                                          out ChargingTariffGroup?  ChargingTariffGroup);

        IVotingNotificator<DateTime, ChargingStationOperator, ChargingTariffGroup, Boolean> ChargingTariffGroupRemoval { get; }

        /// <summary>
        /// Called whenever a charging tariff group will be or was removed.
        /// </summary>
        IVotingSender<DateTime, ChargingStationOperator, ChargingTariffGroup, Boolean> OnChargingTariffGroupRemoval { get; }


        /// <summary>
        /// All charging tariffs registered within this charging station operator.
        /// </summary>
        /// <param name="ChargingTariffGroupId">The unique identification of the charging tariff to be removed.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging tariff after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the charging tariff failed.</param>
        ChargingTariffGroup? RemoveChargingTariffGroup(ChargingTariffGroup_Id                                     ChargingTariffGroupId,
                                                       Action<IChargingStationOperator, ChargingTariffGroup>?     OnSuccess   = null,
                                                       Action<IChargingStationOperator, ChargingTariffGroup_Id>?  OnError     = null);

        /// <summary>
        /// All charging tariffs registered within this charging station operator.
        /// </summary>
        /// <param name="ChargingTariffGroup">The charging tariff to remove.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging tariff after its successful deletion.</param>
        /// <param name="OnError">An optional delegate to be called whenever the deletion of the charging tariff failed.</param>
        ChargingTariffGroup? RemoveChargingTariffGroup(ChargingTariffGroup                                     ChargingTariffGroup,
                                                       Action<IChargingStationOperator, ChargingTariffGroup>?  OnSuccess   = null,
                                                       Action<IChargingStationOperator, ChargingTariffGroup>?  OnError     = null);

        #endregion



        /// <summary>
        /// Return a JSON representation for the given charging station operator.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a roaming network.</param>
        JObject ToJSON(Boolean                                                     Embedded                                  = false,
                       InfoStatus                                                  ExpandRoamingNetworkId                    = InfoStatus.ShowIdOnly,
                       InfoStatus                                                  ExpandChargingPoolIds                     = InfoStatus.ShowIdOnly,
                       InfoStatus                                                  ExpandChargingStationIds                  = InfoStatus.ShowIdOnly,
                       InfoStatus                                                  ExpandEVSEIds                             = InfoStatus.ShowIdOnly,
                       InfoStatus                                                  ExpandBrandIds                            = InfoStatus.ShowIdOnly,
                       InfoStatus                                                  ExpandDataLicenses                        = InfoStatus.ShowIdOnly,
                       CustomJObjectSerializerDelegate<IChargingStationOperator>?  CustomChargingStationOperatorSerializer   = null,
                       CustomJObjectSerializerDelegate<IChargingPool>?             CustomChargingPoolSerializer              = null,
                       CustomJObjectSerializerDelegate<IChargingStation>?          CustomChargingStationSerializer           = null,
                       CustomJObjectSerializerDelegate<IEVSE>?                     CustomEVSESerializer                      = null);


    }

}
