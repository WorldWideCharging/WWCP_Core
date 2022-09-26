﻿/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
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

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP.tests.RoamingNetwork
{

    /// <summary>
    /// Unit tests for charging station operators.
    /// </summary>
    [TestFixture]
    public class ChargingStationOperatorTests : AChargingStationOperatorTests
    {

        #region ChargingStationOperator_Init_Test()

        /// <summary>
        /// A test for creating a charging station operator within a roaming network.
        /// </summary>
        [Test]
        public void ChargingStationOperator_Init_Test()
        {

            Assert.IsNotNull(roamingNetwork);
            Assert.IsNotNull(DE_GEF);

            if (roamingNetwork is not null &&
                DE_GEF         is not null)
            {

                Assert.AreEqual ("DE*GEF",                                              DE_GEF.Id.         ToString());
                Assert.AreEqual ("GraphDefined CSO",                                    DE_GEF.Name.       FirstText());
                Assert.AreEqual ("powered by GraphDefined GmbH",                        DE_GEF.Description.FirstText());

                Assert.AreEqual (ChargingStationOperatorAdminStatusTypes.OutOfService,  DE_GEF.AdminStatus);
                Assert.AreEqual (1,                                                     DE_GEF.AdminStatusSchedule().Count());

                Assert.AreEqual (ChargingStationOperatorStatusTypes.Offline,            DE_GEF.Status);
                Assert.AreEqual (1,                                                     DE_GEF.StatusSchedule().     Count());


                Assert.AreEqual (1,                                                     roamingNetwork.ChargingStationOperators.    Count());
                Assert.AreEqual (1,                                                     roamingNetwork.ChargingStationOperatorIds().Count());


                Assert.IsTrue   (roamingNetwork.ContainsChargingStationOperator(ChargingStationOperator_Id.Parse("DE*GEF")));
                Assert.IsNotNull(roamingNetwork.GetChargingStationOperatorById (ChargingStationOperator_Id.Parse("DE*GEF")));

            }

        }

        #endregion

        #region ChargingStationOperator_Init_DefaultStatus_Test()

        /// <summary>
        /// A test for creating a charging station operator within a roaming network.
        /// </summary>
        [Test]
        public void ChargingStationOperator_Init_DefaultStatus_Test()
        {

            Assert.IsNotNull(roamingNetwork);

            if (roamingNetwork is not null)
            {

                var DE_XXX = roamingNetwork.CreateChargingStationOperator(
                                                Id:           ChargingStationOperator_Id.Parse("DE*XXX"),
                                                Name:         I18NString.Create(Languages.de, "XXX CSO"),
                                                Description:  I18NString.Create(Languages.de, "powered by GraphDefined CSOs GmbH")
                                            );

                Assert.IsNotNull(DE_XXX);

                if (DE_XXX is not null)
                {

                    Assert.AreEqual ("DE*XXX",                                             DE_XXX.Id.         ToString());
                    Assert.AreEqual ("XXX CSO",                                            DE_XXX.Name.       FirstText());
                    Assert.AreEqual ("powered by GraphDefined CSOs GmbH",                  DE_XXX.Description.FirstText());

                    Assert.AreEqual (ChargingStationOperatorAdminStatusTypes.Operational,  DE_XXX.AdminStatus);
                    Assert.AreEqual (ChargingStationOperatorStatusTypes.Available,         DE_XXX.Status);

                    Assert.IsTrue   (roamingNetwork.ContainsChargingStationOperator(ChargingStationOperator_Id.Parse("DE*XXX")));
                    Assert.IsNotNull(roamingNetwork.GetChargingStationOperatorById (ChargingStationOperator_Id.Parse("DE*XXX")));

                }

            }

        }

        #endregion


        #region ChargingStationOperator_AdminStatus_Test()

        /// <summary>
        /// A test for the admin status.
        /// </summary>
        [Test]
        public void ChargingStationOperator_AdminStatus_Test()
        {

            Assert.IsNotNull(roamingNetwork);
            Assert.IsNotNull(DE_GEF);

            if (roamingNetwork is not null &&
                DE_GEF         is not null)
            {

                // Status entries are compared by their ISO 8601 timestamps!
                Thread.Sleep(1000);

                DE_GEF.AdminStatus = ChargingStationOperatorAdminStatusTypes.InternalUse;
                Assert.AreEqual(ChargingStationOperatorAdminStatusTypes.InternalUse,  DE_GEF.AdminStatus);
                Assert.AreEqual("InternalUse, OutOfService",                          DE_GEF.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                Assert.AreEqual(2,                                                    DE_GEF.AdminStatusSchedule().Count());

                Thread.Sleep(1000);

                DE_GEF.AdminStatus = ChargingStationOperatorAdminStatusTypes.Operational;
                Assert.AreEqual(ChargingStationOperatorAdminStatusTypes.Operational,  DE_GEF.AdminStatus);
                Assert.AreEqual("Operational, InternalUse, OutOfService",             DE_GEF.AdminStatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                Assert.AreEqual(3,                                                    DE_GEF.AdminStatusSchedule().Count());

            }

        }

        #endregion

        #region ChargingStationOperator_Status_Test()

        /// <summary>
        /// A test for the admin status.
        /// </summary>
        [Test]
        public void ChargingStationOperator_Status_Test()
        {

            Assert.IsNotNull(roamingNetwork);
            Assert.IsNotNull(DE_GEF);

            if (roamingNetwork is not null &&
                DE_GEF         is not null)
            {

                // Status entries are compared by their ISO 8601 timestamps!
                Thread.Sleep(1000);

                DE_GEF.Status = ChargingStationOperatorStatusTypes.InDeployment;
                Assert.AreEqual(ChargingStationOperatorStatusTypes.InDeployment, DE_GEF.Status);
                Assert.AreEqual("InDeployment, Offline",                         DE_GEF.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                Assert.AreEqual(2,                                               DE_GEF.StatusSchedule().Count());

                Thread.Sleep(1000);

                DE_GEF.Status = ChargingStationOperatorStatusTypes.Faulted;
                Assert.AreEqual(ChargingStationOperatorStatusTypes.Faulted,      DE_GEF.Status);
                Assert.AreEqual("Faulted, InDeployment, Offline",                DE_GEF.StatusSchedule().Select(status => status.Value.ToString()).AggregateWith(", "));
                Assert.AreEqual(3,                                               DE_GEF.StatusSchedule().Count());

            }

        }

        #endregion


    }

}
