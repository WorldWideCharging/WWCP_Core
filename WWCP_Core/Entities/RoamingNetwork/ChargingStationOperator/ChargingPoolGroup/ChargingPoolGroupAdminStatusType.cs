﻿/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The admin status of a charging pool group.
    /// </summary>
    public enum ChargingPoolGroupAdminStatusType
    {

        /// <summary>
        /// Unclear or unknown admin status of the charging pool group.
        /// </summary>
        Unknown             = 0,

        /// <summary>
        /// The charging pool group is out-of-service.
        /// </summary>
        OutOfService        = 1,

        /// <summary>
        /// The charging pool group is available.
        /// </summary>
        Available           = 2,


        /// <summary>
        /// The charging pool group was not found!
        /// (Only valid within batch-processing)
        /// </summary>
        UnknownGroup        = 3

    }

}
