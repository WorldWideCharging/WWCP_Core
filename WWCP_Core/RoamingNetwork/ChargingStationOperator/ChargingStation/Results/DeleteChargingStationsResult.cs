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
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The results of a delete charging stations request.
    /// </summary>
    public class DeleteChargingStationsResult : AEnititiesResult<DeleteChargingStationResult, IChargingStation, ChargingStation_Id>
    {

        #region Constructor(s)

        public DeleteChargingStationsResult(PushDataResultTypes                        Result,
                                            IEnumerable<DeleteChargingStationResult>?  SuccessfulChargingStations   = null,
                                            IEnumerable<DeleteChargingStationResult>?  RejectedChargingStations     = null,
                                            IId?                                       AuthId                       = null,
                                            Object?                                    SendPOIData                  = null,
                                            EventTracking_Id?                          EventTrackingId              = null,
                                            I18NString?                                Description                  = null,
                                            IEnumerable<Warning>?                      Warnings                     = null,
                                            TimeSpan?                                  Runtime                      = null)

            : base(Result,
                   SuccessfulChargingStations,
                   RejectedChargingStations,
                   AuthId,
                   SendPOIData,
                   EventTrackingId,
                   Description,
                   Warnings,
                   Runtime)

        { }

        #endregion


        #region (static) AdminDown    (RejectedChargingStations,   ...)

        public static DeleteChargingStationsResult

            AdminDown(IEnumerable<IChargingStation>  RejectedChargingStations,
                      IId?                           AuthId            = null,
                      Object?                        SendPOIData       = null,
                      EventTracking_Id?              EventTrackingId   = null,
                      I18NString?                    Description       = null,
                      IEnumerable<Warning>?          Warnings          = null,
                      TimeSpan?                      Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.AdminDown,
                        Array.Empty<DeleteChargingStationResult>(),
                        RejectedChargingStations.Select(evse => DeleteChargingStationResult.AdminDown(evse,
                                                                                                      EventTrackingId,
                                                                                                      AuthId,
                                                                                                      SendPOIData)),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) NoOperation  (RejectedChargingStations,   ...)

        public static DeleteChargingStationsResult

            NoOperation(IEnumerable<IChargingStation>  RejectedChargingStations,
                        IId?                           AuthId            = null,
                        Object?                        SendPOIData       = null,
                        EventTracking_Id?              EventTrackingId   = null,
                        I18NString?                    Description       = null,
                        IEnumerable<Warning>?          Warnings          = null,
                        TimeSpan?                      Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.NoOperation,
                        Array.Empty<DeleteChargingStationResult>(),
                        RejectedChargingStations.Select(chargingStation => DeleteChargingStationResult.NoOperation(chargingStation,
                                                                                                                    EventTrackingId,
                                                                                                                    AuthId,
                                                                                                                    SendPOIData)),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


        #region (static) Enqueued     (SuccessfulChargingStations, ...)

        public static DeleteChargingStationsResult

            Enqueued(IEnumerable<IChargingStation>  SuccessfulChargingStations,
                     IId?                           AuthId            = null,
                     Object?                        SendPOIData       = null,
                     EventTracking_Id?              EventTrackingId   = null,
                     I18NString?                    Description       = null,
                     IEnumerable<Warning>?          Warnings          = null,
                     TimeSpan?                      Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.Enqueued,
                        SuccessfulChargingStations.Select(chargingStation => DeleteChargingStationResult.Enqueued(chargingStation,
                                                                                                                  EventTrackingId,
                                                                                                                  AuthId,
                                                                                                                  SendPOIData)),
                        Array.Empty<DeleteChargingStationResult>(),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Success      (SuccessfulChargingStations, ...)

        public static DeleteChargingStationsResult

            Success(IEnumerable<IChargingStation>  SuccessfulChargingStations,
                    IId?                           AuthId            = null,
                    Object?                        SendPOIData       = null,
                    EventTracking_Id?              EventTrackingId   = null,
                    I18NString?                    Description       = null,
                    IEnumerable<Warning>?          Warnings          = null,
                    TimeSpan?                      Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.Success,
                        SuccessfulChargingStations.Select(chargingStation => DeleteChargingStationResult.Success(chargingStation,
                                                                                                                 EventTrackingId,
                                                                                                                 AuthId,
                                                                                                                 SendPOIData)),
                        Array.Empty<DeleteChargingStationResult>(),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


        #region (static) ArgumentError(RejectedChargingStations, Description, ...)

        public static DeleteChargingStationsResult

            ArgumentError(IEnumerable<IChargingStation>  RejectedChargingStations,
                          I18NString                     Description,
                          EventTracking_Id?              EventTrackingId   = null,
                          IId?                           AuthId            = null,
                          Object?                        SendPOIData       = null,
                          IEnumerable<Warning>?          Warnings          = null,
                          TimeSpan?                      Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.ArgumentError,
                        Array.Empty<DeleteChargingStationResult>(),
                        RejectedChargingStations.Select(chargingStation => DeleteChargingStationResult.ArgumentError(chargingStation,
                                                                                                                     Description,
                                                                                                                     EventTrackingId,
                                                                                                                     AuthId,
                                                                                                                     SendPOIData)),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Error        (RejectedChargingStations, Description, ...)

        public static DeleteChargingStationsResult

            Error(IEnumerable<IChargingStation>  RejectedChargingStations,
                  I18NString                     Description,
                  EventTracking_Id?              EventTrackingId   = null,
                  IId?                           AuthId            = null,
                  Object?                        SendPOIData       = null,
                  IEnumerable<Warning>?          Warnings          = null,
                  TimeSpan?                      Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.Error,
                        Array.Empty<DeleteChargingStationResult>(),
                        RejectedChargingStations.Select(chargingStation => DeleteChargingStationResult.Error(chargingStation,
                                                                                                             Description,
                                                                                                             EventTrackingId,
                                                                                                             AuthId,
                                                                                                             SendPOIData)),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Error        (RejectedChargingStations, Exception,   ...)

        public static DeleteChargingStationsResult

            Error(IEnumerable<IChargingStation>  RejectedChargingStations,
                  Exception                      Exception,
                  EventTracking_Id?              EventTrackingId   = null,
                  IId?                           AuthId            = null,
                  Object?                        SendPOIData       = null,
                  IEnumerable<Warning>?          Warnings          = null,
                  TimeSpan?                      Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.Error,
                        Array.Empty<DeleteChargingStationResult>(),
                        RejectedChargingStations.Select(chargingStation => DeleteChargingStationResult.Error(chargingStation,
                                                                                                             Exception,
                                                                                                             EventTrackingId,
                                                                                                             AuthId,
                                                                                                             SendPOIData)),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Exception.Message.ToI18NString(),
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) LockTimeout  (RejectedChargingStations, Timeout, ...)

        public static DeleteChargingStationsResult

            LockTimeout(IEnumerable<IChargingStation>  RejectedChargingStations,
                        TimeSpan                       Timeout,
                        IId?                           AuthId            = null,
                        Object?                        SendPOIData       = null,
                        EventTracking_Id?              EventTrackingId   = null,
                        I18NString?                    Description       = null,
                        IEnumerable<Warning>?          Warnings          = null,
                        TimeSpan?                      Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.LockTimeout,
                        Array.Empty<DeleteChargingStationResult>(),
                        RejectedChargingStations.Select(chargingStation => DeleteChargingStationResult.LockTimeout(chargingStation,
                                                                                                                   Timeout,
                                                                                                                   EventTrackingId,
                                                                                                                   AuthId,
                                                                                                                   SendPOIData)),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


    }

}
