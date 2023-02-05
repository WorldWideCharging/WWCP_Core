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

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A charging station status update.
    /// </summary>
    public readonly struct ChargingStationStatusUpdate : IEquatable<ChargingStationStatusUpdate>,
                                                         IComparable<ChargingStationStatusUpdate>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging station.
        /// </summary>
        public ChargingStation_Id                       Id          { get; }

        /// <summary>
        /// The old timestamped status of the charging station.
        /// </summary>
        public Timestamped<ChargingStationStatusTypes>  OldStatus   { get; }

        /// <summary>
        /// The new timestamped status of the charging station.
        /// </summary>
        public Timestamped<ChargingStationStatusTypes>  NewStatus   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station status update.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        /// <param name="OldStatus">The old timestamped status of the charging station.</param>
        /// <param name="NewStatus">The new timestamped status of the charging station.</param>
        public ChargingStationStatusUpdate(ChargingStation_Id                       Id,
                                           Timestamped<ChargingStationStatusTypes>  OldStatus,
                                           Timestamped<ChargingStationStatusTypes>  NewStatus)

        {

            this.Id         = Id;
            this.OldStatus  = OldStatus;
            this.NewStatus  = NewStatus;

        }

        #endregion


        #region (static) Snapshot(ChargingStation)

        /// <summary>
        /// Take a snapshot of the current charging station status.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public static ChargingStationStatusUpdate Snapshot(IChargingStation ChargingStation)

            => new (ChargingStation.Id,
                    ChargingStation.Status,
                    ChargingStation.StatusSchedule().Skip(1).FirstOrDefault());

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationStatusUpdate1, ChargingStationStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusUpdate1">A charging station status update.</param>
        /// <param name="ChargingStationStatusUpdate2">Another charging station status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationStatusUpdate ChargingStationStatusUpdate1,
                                           ChargingStationStatusUpdate ChargingStationStatusUpdate2)

            => ChargingStationStatusUpdate1.Equals(ChargingStationStatusUpdate2);

        #endregion

        #region Operator != (ChargingStationStatusUpdate1, ChargingStationStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusUpdate1">A charging station status update.</param>
        /// <param name="ChargingStationStatusUpdate2">Another charging station status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationStatusUpdate ChargingStationStatusUpdate1,
                                           ChargingStationStatusUpdate ChargingStationStatusUpdate2)

            => !ChargingStationStatusUpdate1.Equals(ChargingStationStatusUpdate2);

        #endregion

        #region Operator <  (ChargingStationStatusUpdate1, ChargingStationStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusUpdate1">A charging station status update.</param>
        /// <param name="ChargingStationStatusUpdate2">Another charging station status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationStatusUpdate ChargingStationStatusUpdate1,
                                          ChargingStationStatusUpdate ChargingStationStatusUpdate2)

            => ChargingStationStatusUpdate1.CompareTo(ChargingStationStatusUpdate2) < 0;

        #endregion

        #region Operator <= (ChargingStationStatusUpdate1, ChargingStationStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusUpdate1">A charging station status update.</param>
        /// <param name="ChargingStationStatusUpdate2">Another charging station status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationStatusUpdate ChargingStationStatusUpdate1,
                                           ChargingStationStatusUpdate ChargingStationStatusUpdate2)

            => ChargingStationStatusUpdate1.CompareTo(ChargingStationStatusUpdate2) <= 0;

        #endregion

        #region Operator >  (ChargingStationStatusUpdate1, ChargingStationStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusUpdate1">A charging station status update.</param>
        /// <param name="ChargingStationStatusUpdate2">Another charging station status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationStatusUpdate ChargingStationStatusUpdate1,
                                          ChargingStationStatusUpdate ChargingStationStatusUpdate2)

            => ChargingStationStatusUpdate1.CompareTo(ChargingStationStatusUpdate2) > 0;

        #endregion

        #region Operator >= (ChargingStationStatusUpdate1, ChargingStationStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatusUpdate1">A charging station status update.</param>
        /// <param name="ChargingStationStatusUpdate2">Another charging station status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationStatusUpdate ChargingStationStatusUpdate1,
                                           ChargingStationStatusUpdate ChargingStationStatusUpdate2)

            => ChargingStationStatusUpdate1.CompareTo(ChargingStationStatusUpdate2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingStationStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging station status updates.
        /// </summary>
        /// <param name="Object">A charging station status update to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is ChargingStationStatusUpdate chargingStationStatusUpdate
                   ? CompareTo(chargingStationStatusUpdate)
                   : throw new ArgumentException("The given object is not a charging station status update!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationStatusUpdate)

        /// <summary>
        /// Compares two charging station status updates.
        /// </summary>
        /// <param name="ChargingStationStatusUpdate">A charging station status update to compare with.</param>
        public Int32 CompareTo(ChargingStationStatusUpdate ChargingStationStatusUpdate)
        {

            var c = Id.       CompareTo(ChargingStationStatusUpdate.Id);

            if (c == 0)
                c = NewStatus.CompareTo(ChargingStationStatusUpdate.NewStatus);

            if (c == 0)
                c = OldStatus.CompareTo(ChargingStationStatusUpdate.OldStatus);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationStatusUpdate> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging station status updates for equality.
        /// </summary>
        /// <param name="Object">A charging station status update to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationStatusUpdate chargingStationStatusUpdate &&
                   Equals(chargingStationStatusUpdate);

        #endregion

        #region Equals(ChargingStationStatusUpdate)

        /// <summary>
        /// Compares two charging station status updates for equality.
        /// </summary>
        /// <param name="ChargingStationStatusUpdate">A charging station status update to compare with.</param>
        public Boolean Equals(ChargingStationStatusUpdate ChargingStationStatusUpdate)

            => Id.       Equals(ChargingStationStatusUpdate.Id)        &&
               OldStatus.Equals(ChargingStationStatusUpdate.OldStatus) &&
               NewStatus.Equals(ChargingStationStatusUpdate.NewStatus);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return Id.       GetHashCode() * 5 ^
                       OldStatus.GetHashCode() * 3 ^
                       NewStatus.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Id, ": ",
                             OldStatus,
                             " -> ",
                             NewStatus);

        #endregion

    }

}
