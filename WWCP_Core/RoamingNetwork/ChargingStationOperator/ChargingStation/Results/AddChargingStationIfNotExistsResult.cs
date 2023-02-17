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

    public class AddChargingStationIfNotExistsResult : AEnitityResult<ChargingStation, ChargingStation_Id>
    {

        public ChargingStation? ChargingStation
            => Object;

        public ChargingStationOperator?  ChargingStationOperator    { get; internal set; }

        public AddedOrIgnored?           AddedOrIgnored             { get; internal set; }


        public AddChargingStationIfNotExistsResult(ChargingStation           ChargingStation,
                                                   EventTracking_Id          EventTrackingId,
                                                   Boolean                   IsSuccess,
                                                   String?                   Argument                  = null,
                                                   I18NString?               ErrorDescription          = null,
                                                   ChargingStationOperator?  ChargingStationOperator   = null,
                                                   AddedOrIgnored?           AddedOrIgnored            = null)

            : base(ChargingStation,
                   EventTrackingId,
                   IsSuccess,
                   Argument,
                   ErrorDescription)

        {

            this.ChargingStationOperator  = ChargingStationOperator;
            this.AddedOrIgnored           = AddedOrIgnored;

        }


        public static AddChargingStationIfNotExistsResult Success(ChargingStation           ChargingStation,
                                                                  AddedOrIgnored            AddedOrIgnored,
                                                                  EventTracking_Id          EventTrackingId,
                                                                  ChargingStationOperator?  ChargingStationOperator   = null)

            => new (ChargingStation,
                    EventTrackingId,
                    true,
                    null,
                    null,
                    ChargingStationOperator,
                    AddedOrIgnored);


        public static AddChargingStationIfNotExistsResult ArgumentError(ChargingStation   ChargingStation,
                                                                        EventTracking_Id  EventTrackingId,
                                                                        String            Argument,
                                                                        String            Description)

            => new (ChargingStation,
                    EventTrackingId,
                    false,
                    Argument,
                    I18NString.Create(
                        Languages.en,
                        Description
                    ));

        public static AddChargingStationIfNotExistsResult ArgumentError(ChargingStation   ChargingStation,
                                                                        EventTracking_Id  EventTrackingId,
                                                                        String            Argument,
                                                                        I18NString        Description)

            => new (ChargingStation,
                    EventTrackingId,
                    false,
                    Argument,
                    Description);


        public static AddChargingStationIfNotExistsResult Failed(ChargingStation           ChargingStation,
                                                                 EventTracking_Id          EventTrackingId,
                                                                 String                    Description,
                                                                 ChargingStationOperator?  ChargingStationOperator   = null)

            => new (ChargingStation,
                    EventTrackingId,
                    false,
                    null,
                    I18NString.Create(
                        Languages.en,
                        Description
                    ),
                    ChargingStationOperator);

        public static AddChargingStationIfNotExistsResult Failed(ChargingStation           ChargingStation,
                                                                 EventTracking_Id          EventTrackingId,
                                                                 I18NString                Description,
                                                                 ChargingStationOperator?  ChargingStationOperator   = null)

            => new (ChargingStation,
                    EventTrackingId,
                    false,
                    null,
                    Description,
                    ChargingStationOperator);

        public static AddChargingStationIfNotExistsResult Failed(ChargingStation           ChargingStation,
                                                                 EventTracking_Id          EventTrackingId,
                                                                 Exception                 Exception,
                                                                 ChargingStationOperator?  ChargingStationOperator   = null)

            => new (ChargingStation,
                    EventTrackingId,
                    false,
                    null,
                    I18NString.Create(
                        Languages.en,
                        Exception.Message
                    ),
                    ChargingStationOperator);

    }

}
