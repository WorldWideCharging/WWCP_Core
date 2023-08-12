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

using org.GraphDefined.Vanaheimr.Illias;

using social.OpenData.UsersAPI;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public class AddOrUpdateChargingPoolResult : AEnitityResult<IChargingPool, ChargingPool_Id>
    {

        #region Properties

        public IChargingPool?             ChargingPool
            => Object;

        public IChargingStationOperator?  ChargingStationOperator    { get; internal set; }

        public AddedOrUpdated?            AddedOrUpdated             { get; internal set; }

        #endregion

        #region Constructor(s)

        public AddOrUpdateChargingPoolResult(IChargingPool              ChargingPool,
                                             PushDataResultTypes        Result,
                                             EventTracking_Id?          EventTrackingId           = null,
                                             IId?                       AuthId                    = null,
                                             Object?                    SendPOIData               = null,
                                             IChargingStationOperator?  ChargingStationOperator   = null,
                                             AddedOrUpdated?            AddedOrUpdated            = null,
                                             I18NString?                Description               = null,
                                             IEnumerable<Warning>?      Warnings                  = null,
                                             TimeSpan?                  Runtime                   = null)

            : base(ChargingPool,
                   Result,
                   EventTrackingId,
                   AuthId,
                   SendPOIData,
                   Description,
                   Warnings,
                   Runtime)

        {

            this.ChargingStationOperator  = ChargingStationOperator;
            this.AddedOrUpdated           = AddedOrUpdated;

        }

        #endregion


        #region (static) NoOperation

        public static AddOrUpdateChargingPoolResult

            NoOperation(IChargingPool              ChargingPool,
                        EventTracking_Id?          EventTrackingId           = null,
                        IId?                       AuthId                    = null,
                        Object?                    SendPOIData               = null,
                        IChargingStationOperator?  ChargingStationOperator   = null,
                        I18NString?                Description               = null,
                        IEnumerable<Warning>?      Warnings                  = null,
                        TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        PushDataResultTypes.NoOperation,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingStationOperator,
                        social.OpenData.UsersAPI.AddedOrUpdated.NoOperation,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) ArgumentError(...)

        public static AddOrUpdateChargingPoolResult

            ArgumentError(IChargingPool              ChargingPool,
                          EventTracking_Id?          EventTrackingId           = null,
                          IId?                       AuthId                    = null,
                          Object?                    SendPOIData               = null,
                          IChargingStationOperator?  ChargingStationOperator   = null,
                          I18NString?                Description               = null,
                          IEnumerable<Warning>?      Warnings                  = null,
                          TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        PushDataResultTypes.ArgumentError,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingStationOperator,
                        social.OpenData.UsersAPI.AddedOrUpdated.Failed,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) Added(...)

        public static AddOrUpdateChargingPoolResult

            Added(IChargingPool              ChargingPool,
                  EventTracking_Id?          EventTrackingId           = null,
                  IId?                       AuthId                    = null,
                  Object?                    SendPOIData               = null,
                  IChargingStationOperator?  ChargingStationOperator   = null,
                  I18NString?                Description               = null,
                  IEnumerable<Warning>?      Warnings                  = null,
                  TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        PushDataResultTypes.Success,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingStationOperator,
                        social.OpenData.UsersAPI.AddedOrUpdated.Add,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Updated(...)

        public static AddOrUpdateChargingPoolResult

            Updated(IChargingPool              ChargingPool,
                    EventTracking_Id?          EventTrackingId           = null,
                    IId?                       AuthId                    = null,
                    Object?                    SendPOIData               = null,
                    IChargingStationOperator?  ChargingStationOperator   = null,
                    I18NString?                Description               = null,
                    IEnumerable<Warning>?      Warnings                  = null,
                    TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        PushDataResultTypes.Success,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingStationOperator,
                        social.OpenData.UsersAPI.AddedOrUpdated.Update,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Enqueued(...)

        public static AddOrUpdateChargingPoolResult

            Enqueued(IChargingPool              ChargingPool,
                     EventTracking_Id?          EventTrackingId           = null,
                     IId?                       AuthId                    = null,
                     Object?                    SendPOIData               = null,
                     IChargingStationOperator?  ChargingStationOperator   = null,
                     I18NString?                Description               = null,
                     IEnumerable<Warning>?      Warnings                  = null,
                     TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        PushDataResultTypes.Enqueued,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingStationOperator,
                        social.OpenData.UsersAPI.AddedOrUpdated.Enqueued,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) Error(ChargingStationOperator, Description, ...)

        public static AddOrUpdateChargingPoolResult

            Error(IChargingPool              ChargingPool,
                  I18NString                 Description,
                  EventTracking_Id?          EventTrackingId           = null,
                  IId?                       AuthId                    = null,
                  Object?                    SendPOIData               = null,
                  IChargingStationOperator?  ChargingStationOperator   = null,
                  IEnumerable<Warning>?      Warnings                  = null,
                  TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        PushDataResultTypes.Error,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingStationOperator,
                        social.OpenData.UsersAPI.AddedOrUpdated.Update,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error(ChargingStationOperator, Exception,   ...)

        public static AddOrUpdateChargingPoolResult

            Error(IChargingPool              ChargingPool,
                  Exception                  Exception,
                  EventTracking_Id?          EventTrackingId           = null,
                  IId?                       AuthId                    = null,
                  Object?                    SendPOIData               = null,
                  IChargingStationOperator?  ChargingStationOperator   = null,
                  IEnumerable<Warning>?      Warnings                  = null,
                  TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        PushDataResultTypes.Error,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingStationOperator,
                        social.OpenData.UsersAPI.AddedOrUpdated.Update,
                        I18NString.Create(
                            Languages.en,
                            Exception.Message
                        ),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) LockTimeout(Timeout, ...)

        public static AddOrUpdateChargingPoolResult

            LockTimeout(IChargingPool              ChargingPool,
                        TimeSpan                   Timeout,
                        EventTracking_Id?          EventTrackingId           = null,
                        IId?                       AuthId                    = null,
                        Object?                    SendPOIData               = null,
                        IChargingStationOperator?  ChargingStationOperator   = null,
                        IEnumerable<Warning>?      Warnings                  = null,
                        TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        PushDataResultTypes.LockTimeout,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingStationOperator,
                        social.OpenData.UsersAPI.AddedOrUpdated.Failed,
                        I18NString.Create(
                            Languages.en,
                            $"Lock timeout after {Timeout.TotalSeconds} seconds!"
                        ),
                        Warnings,
                        Runtime);

        #endregion


    }

}
