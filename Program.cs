using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FlightManagement
{
    class Program
    {
        List<FLightsSchedule> FLightsSchedules = new List<FLightsSchedule>();
        static void Main(string[] args)
        {
            string flightSchedule = LoadFlightSchedule();
            string flightIternary = GenerateFlightIternaries();
            Console.WriteLine(flightSchedule);
            Console.WriteLine(flightIternary);
        }
        //user story 1
        //To Load all the flight schedule
        private static string LoadFlightSchedule()
        {
            string flightSchedule = string.Empty;

            for (int i = 0; i < fLightsSchedules.Count; i++)
            {
                foreach (var flightDetails in fLightsSchedules[i].flightDetails)
                    flightSchedule += String.Format("Flight: {0}, departure: {1}, arrival: {2}, day: {3} \n",
                        flightDetails.flightNumber,
                        flightDetails.sourceCityCode,
                        flightDetails.destinationCityCode,
                        fLightsSchedules[i].day
                        );
            }
            return flightSchedule;
        }

        //user story 2
        //To generate all flight Iternries
        private static string GenerateFlightIternaries()
        {
            string flightIternaries = string.Empty;
            //Read the data from Given Json File
            JObject o1 = JObject.Parse(File.ReadAllText(@"C:\Users\polav\source\repos\FlightManagement\FlightManagement\coding-assigment-orders.json"));
            string OrderDestinations = JsonConvert.SerializeObject(o1);
            //regular expression to pull out order numbers
            string order = Regex.Replace(OrderDestinations, "[^0-9:]", "");
            string[] orders = order.Split("::");
            //regular expression to pull out destination codes
            Regex pattern = new Regex("[order -:{destination]");
            string destination = pattern.Replace(OrderDestinations, string.Empty);
            string[] destinations = destination.Split("}");
            List<OrderDetails> orderDetails = new List<OrderDetails>();
            int orderNumber = 1;
            for (int i = 0; i < destinations.Length; i++)
            {
                if (destinations[i] != string.Empty && orders[i] != string.Empty)
                {
                    orderDetails.Add(new OrderDetails { orderNumber = orders[i], Destination = destinations[i] });
                    orderNumber++;
                }
            }
            List<string> ordersAlreadyScheduled = new List<string>();
            //below logic to load all the scheduled orders
            foreach (var fLightsSchedule in fLightsSchedules)
            {
                foreach (var flightDetail in fLightsSchedule.flightDetails)
                {
                    for (int i = 0; i < flightDetail.flightCapacity; i++)
                    {
                        var scheduledOrders = (from r in orderDetails
                                               where r.Destination == flightDetail.destinationCityCode && ordersAlreadyScheduled.Contains(r.orderNumber) == false
                                               select new OrderDetails { orderNumber = r.orderNumber, Destination = r.Destination }).ToList();
                        if (scheduledOrders.Count != 0)
                        {
                            flightIternaries += String.Format("order: order-{0}, flightNumber: {1}, departure: {2}, arrival: {3}, day: {4} \n",
                               scheduledOrders[0].orderNumber,
                               flightDetail.flightNumber,
                               flightDetail.sourceCityCode,
                               flightDetail.destinationCityCode,
                               fLightsSchedule.day
                               );
                            ordersAlreadyScheduled.Add(scheduledOrders[0].orderNumber);
                        }
                    }
                }
            }
            //below logic to find out orders not scheduled
            var ordersNotScheduled = (from r in orderDetails
                                      where ordersAlreadyScheduled.Contains(r.orderNumber) == false
                                      select r.orderNumber).ToList();
            if (ordersNotScheduled.Count != 0)
            {
                foreach (var ordernotscheduled in ordersNotScheduled)
                {
                    flightIternaries += String.Format("order: order-{0}, flightNumber: not scheduled \n", ordernotscheduled);
                }
            }
            return flightIternaries;
        }

        //Load given data 
        static List<FLightsSchedule> fLightsSchedules = new List<FLightsSchedule>
        {
            new FLightsSchedule
            {
                day = 1,
                flightDetails = new List<FlightDetails>
                {
                    new FlightDetails
                    {
                        flightNumber = 1,
                        flightCapacity = 20,
                        sourceCity = "Montreal",
                        sourceCityCode = "YUL",
                        destinationCity = "Toronto",
                        destinationCityCode = "YYZ"
                    },
                    new FlightDetails
                    {
                        flightNumber = 2,
                        flightCapacity = 20,
                        sourceCity = "Montreal",
                        sourceCityCode = "YUL",
                        destinationCity = "Calgary",
                        destinationCityCode = "YYC"
                    },
                    new FlightDetails
                    {
                        flightNumber = 3,
                        flightCapacity = 20,
                        sourceCity = "Montreal",
                        sourceCityCode = "YUL",
                        destinationCity = "Vancouver",
                        destinationCityCode = "YVR"
                    }
                }
            },
            new FLightsSchedule
            {
                day = 2,
                flightDetails = new List<FlightDetails>
                {
                    new FlightDetails
                    {
                        flightNumber = 4,
                        flightCapacity = 20,
                        sourceCity = "Montreal",
                        sourceCityCode = "YUL",
                        destinationCity = "Toronto",
                        destinationCityCode = "YYZ"
                    },
                    new FlightDetails
                    {
                        flightNumber = 5,
                        flightCapacity = 20,
                        sourceCity = "Montreal",
                        sourceCityCode = "YUL",
                        destinationCity = "Calgary",
                        destinationCityCode = "YYC"
                    },
                    new FlightDetails
                    {
                        flightNumber = 6,
                        flightCapacity = 20,
                        sourceCity = "Montreal",
                        sourceCityCode = "YUL",
                        destinationCity = "Vancouver",
                        destinationCityCode = "YVR"
                    }
                }
            }
        };

    }
}
