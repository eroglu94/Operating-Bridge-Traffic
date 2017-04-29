using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Windows.Threading;

namespace Operating_Project_Bridge
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        DispatcherTimer CarGeneratorTimer = new System.Windows.Threading.DispatcherTimer();
        DispatcherTimer RefreshUI = new System.Windows.Threading.DispatcherTimer();
        DispatcherTimer MoveCarTimer = new System.Windows.Threading.DispatcherTimer();


        public MainWindow()
        {
            InitializeComponent();


            CarGeneratorTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            CarGeneratorTimer.Interval = new TimeSpan(0, 0, 0, 0, 333);

            RefreshUI.Tick += new EventHandler(RefreshUI_Tick);
            RefreshUI.Interval = new TimeSpan(0, 0, 0, 0, 102);

            MoveCarTimer.Tick += new EventHandler(MoveCarTimer_Tick);
            MoveCarTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);

            SetCarQueueImages();

            //Start Conditions
            TrafficLamp(1, false);
            TrafficLamp(2, false);
            TrafficLamp(3, false);
        }

        private void MoveCarTimer_Tick(object sender, EventArgs e)
        {
            MoveCars();
        }

        private async void RefreshUI_Tick(object sender, EventArgs e)
        {

            await RefreshUIFunc();
        }

        public async Task RefreshUIFunc()
        {
            if (Light_Green_1.IsVisible)
            {
                roadQ1.flowTime = roadQ1.flowTime + 100;
                roadQ1.lampStatus = true;
            }
            else
            {

                roadQ1.lampStatus = false;
            }


            if (Light_Green_2.IsVisible)
            {
                roadQ2.flowTime = roadQ2.flowTime + 100;
                roadQ2.lampStatus = true;
            }
            else
            {

                roadQ2.lampStatus = false;
            }


            if (Light_Green_3.IsVisible)
            {
                roadQ3.flowTime = roadQ3.flowTime + 100;
                roadQ3.lampStatus = true;
            }
            else
            {

                roadQ3.lampStatus = false;
            }

            var x = new RoadQueue[3];
            x[0] = roadQ1; x[1] = roadQ2; x[2] = roadQ3;

            var temp = x.Max(p => p.flowTime);
            if (temp == x[0].flowTime)
            {
                BiggestFlowTime = 1;
            }
            if (temp == x[1].flowTime)
            {
                BiggestFlowTime = 2;
            }
            if (temp == x[2].flowTime)
            {
                BiggestFlowTime = 3;
            }

            //Images
            ResetQueueImages();

            //Queue
            for (int i = 1; i <= roadQ1.carsWaiting; i++)
            {
                if (i >= 5) { break; }

                roadQ1.image[i - 1].Visibility = Visibility.Visible;
            }

            for (int i = 1; i <= roadQ2.carsWaiting; i++)
            {
                if (i >= 5) { break; }

                roadQ2.image[i - 1].Visibility = Visibility.Visible;
            }

            for (int i = 1; i <= roadQ3.carsWaiting; i++)
            {
                if (i >= 5) { break; }

                roadQ3.image[i - 1].Visibility = Visibility.Visible;
            }



            //Bridge
            for (int i = 0; i <= 2; i++)
            {
                if (carLoc1[i].isCarExists == true)
                {
                    roadQ1.image[i + 4].Visibility = Visibility.Visible;
                }

                if (carLoc3[i].isCarExists == true)
                {
                    roadQ3.image[i + 4].Visibility = Visibility.Visible;
                }
            }

            for (int i = 0; i <= 5; i++)
            {
                if (carLoc2[i].isCarExists == true)
                {
                    if (i == 6)
                    {

                    }
                    roadQ2.image[i + 4].Visibility = Visibility.Visible;
                }
            }



            //CAR COUNT
            lbl_redcars.Text = roadQ1.totalGenerated.ToString();
            lbl_bluecars.Text = roadQ2.totalGenerated.ToString();
            lbl_greencars.Text = roadQ3.totalGenerated.ToString();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {

            GenerateCars();
            CalculateLamp();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            CarGeneratorTimer.Start();
            RefreshUI.Start();
            MoveCarTimer.Start();

        }



        //@@@@@@@@@@@     ALGORITHMS     @@@@@@@@@@@

        public struct RoadQueue
        {
            public int carsWaiting { get; set; }
            public int bridgeCars { get; set; }
            public int totalGenerated { get; set; }
            public int flowTime { get; set; }
            public bool lampStatus { get; set; }
            public UIElement[] image { get; set; }

        }

        public struct CarLocation
        {
            public int carType { get; set; }
            public bool isCarExists { get; set; }
            public Point Location { get; set; }
        }


        //Global Variables
        RoadQueue roadQ1 = new RoadQueue();
        RoadQueue roadQ2 = new RoadQueue();
        RoadQueue roadQ3 = new RoadQueue();
        //BridgeTraffic[,] bridgeTraffic = new BridgeTraffic[2, 3];
        CarLocation[] carLoc1 = new CarLocation[3];
        CarLocation[] carLoc2 = new CarLocation[7];
        CarLocation[] carLoc3 = new CarLocation[3];
        int LastGeneratedCar = 0;
        int TotalGeneratedCars = 0;
        double BiggestFlowTime = 0;


        public void TrafficLamp(int LampNumber, bool status)
        {
            if (LampNumber == 1)
            {
                if (status == true)
                {
                    Light_Green_1.Visibility = Visibility.Visible;
                }
                else
                {
                    Light_Green_1.Visibility = Visibility.Hidden;
                }
            }

            if (LampNumber == 2)
            {
                if (status == true)
                {
                    Light_Green_2.Visibility = Visibility.Visible;
                }
                else
                {
                    Light_Green_2.Visibility = Visibility.Hidden;
                }
            }

            if (LampNumber == 3)
            {
                if (status == true)
                {
                    Light_Green_3.Visibility = Visibility.Visible;
                }
                else
                {
                    Light_Green_3.Visibility = Visibility.Hidden;
                }
            }
        }

        public void SetCarQueueImages()
        {
            //CAR IMAGES
            var temp1 = new RoadQueue
            {
                image = new UIElement[8]
            };
            var temp2 = new RoadQueue
            {
                image = new UIElement[10]
            };
            var temp3 = new RoadQueue
            {
                image = new UIElement[8]
            };

            temp1.image[0] = Road1_1;
            temp1.image[1] = Road1_2;
            temp1.image[2] = Road1_3;
            temp1.image[3] = Road1_4;
            temp1.image[4] = Road1_5;
            temp1.image[5] = Road1_6;
            temp1.image[6] = Road1_7;
            temp1.image[7] = Road1_8;

            temp2.image[0] = Road2_1;
            temp2.image[1] = Road2_2;
            temp2.image[2] = Road2_3;
            temp2.image[3] = Road2_4;

            temp2.image[4] = Road2_5_down;
            temp2.image[5] = Road2_6_down;
            temp2.image[6] = Road2_7_down;

            //temp2.image[7] = Road2_8_down;

            temp2.image[7] = Road2_5_up;
            temp2.image[8] = Road2_6_up;
            temp2.image[9] = Road2_7_up;

            //temp2.image[11] = Road2_8_up;

            temp3.image[0] = Road3_1;
            temp3.image[1] = Road3_2;
            temp3.image[2] = Road3_3;
            temp3.image[3] = Road3_4;
            temp3.image[4] = Road3_5;
            temp3.image[5] = Road3_6;
            temp3.image[6] = Road3_7;
            temp3.image[7] = Road3_8;


            roadQ1 = temp1;
            roadQ2 = temp2;
            roadQ3 = temp3;
        }

        public void ResetQueueImages()
        {
            for (int i = 0; i <= 9; i++)
            {
                if (i <= 7)
                {
                    roadQ1.image[i].Visibility = Visibility.Hidden;
                    roadQ3.image[i].Visibility = Visibility.Hidden;
                }

                roadQ2.image[i].Visibility = Visibility.Hidden;
            }
        }

        public void GenerateCars()
        {
            TotalGeneratedCars++;
            if (TotalGeneratedCars >= 61)
            {
                //MessageBox.Show("Done Generating Cars");
                TrafficLamp(1, true);
                TrafficLamp(2, true);
                TrafficLamp(3, true);
                CarGeneratorTimer.Stop();
                return;
            }

            AGAIN:
            var randomNumber = new Random().Next(1, 4);



            if (randomNumber == 1)
            {
                if (roadQ1.totalGenerated >= 20)
                {
                    goto AGAIN;
                }
                roadQ1.carsWaiting++;
                roadQ1.totalGenerated++;
                LastGeneratedCar = 1;

            }
            if (randomNumber == 2)
            {
                if (roadQ2.totalGenerated >= 20)
                {
                    goto AGAIN;
                }
                roadQ2.carsWaiting++;
                roadQ2.totalGenerated++;
                LastGeneratedCar = 1;
            }
            if (randomNumber == 3)
            {
                if (roadQ3.totalGenerated >= 20)
                {
                    goto AGAIN;
                }
                roadQ3.carsWaiting++;
                roadQ3.totalGenerated++;
                LastGeneratedCar = 1;
            }

        }

        public async void CalculateLamp()
        {

            //Condition 1 No cars on other road
            if (!Condition1())
            {
                Condition2();
            }


            var check = 0;
            if (!Light_Green_1.IsVisible)
            {
                check++;
            }
            if (!Light_Green_2.IsVisible)
            {
                check++;
            }
            if (!Light_Green_3.IsVisible)
            {
                check++;
            }

            if (check >= 2)
            {
                if (roadQ2.carsWaiting > roadQ3.carsWaiting)
                {
                    TrafficLamp(2, true);
                }
                else if (roadQ2.carsWaiting < roadQ3.carsWaiting)
                {
                    TrafficLamp(3, true);
                }
                if (roadQ1.carsWaiting > roadQ3.carsWaiting)
                {
                    TrafficLamp(1, true);
                }
            }


            await RefreshUIFunc();

            if (roadQ1.lampStatus == false)
            {
                roadQ1.flowTime = 0;
            }

            if (roadQ2.lampStatus == false)
            {
                roadQ2.flowTime = 0;
            }

            if (roadQ3.lampStatus == false)
            {
                roadQ3.flowTime = 0;
            }
        }

        public bool Condition1()
        {
            if (LastGeneratedCar == 1)
            {
                if (roadQ2.carsWaiting == 0)
                {
                    TrafficLamp(1, true);
                    TrafficLamp(2, false);
                    return true;
                }
                if (roadQ3.carsWaiting == 0)
                {
                    TrafficLamp(1, true);
                    TrafficLamp(3, false);
                    return true;
                }


                TrafficLamp(1, false);

            }

            if (LastGeneratedCar == 2)
            {
                if (roadQ1.carsWaiting == 0)
                {
                    TrafficLamp(2, true);
                    TrafficLamp(1, false);
                    return true;
                }
                if (roadQ3.carsWaiting == 0)
                {
                    TrafficLamp(2, true);
                    TrafficLamp(3, false);
                    return true;
                }

                TrafficLamp(2, false);
            }

            if (LastGeneratedCar == 3)
            {
                if (roadQ1.carsWaiting == 0)
                {
                    TrafficLamp(3, true);
                    TrafficLamp(1, false);
                    return true;
                }
                if (roadQ2.carsWaiting == 0)
                {
                    TrafficLamp(3, true);
                    TrafficLamp(2, false);
                    return true;
                }


                TrafficLamp(3, false);

            }

            return false;
        }

        public void Condition2()
        {
            var bigflow = BiggestFlowTime;


            if (LastGeneratedCar == 1)
            {
                if (roadQ1.carsWaiting >= 3)
                {
                    if (bigflow == 2)
                    {
                        TrafficLamp(2, false);
                    }
                    if (bigflow == 3)
                    {
                        TrafficLamp(3, false);
                    }
                    TrafficLamp(1, true);
                }
                else
                {
                    TrafficLamp(2, true);
                    TrafficLamp(3, true);
                }

            }

            if (LastGeneratedCar == 2)
            {
                if (roadQ2.carsWaiting >= 3)
                {
                    if (bigflow == 1)
                    {
                        TrafficLamp(1, false);
                    }
                    if (bigflow == 3)
                    {
                        TrafficLamp(3, false);
                    }
                    TrafficLamp(2, true);
                }
                else
                {
                    TrafficLamp(1, true);
                    TrafficLamp(3, true);
                }
            }

            if (LastGeneratedCar == 3)
            {
                if (roadQ3.carsWaiting >= 3)
                {
                    if (bigflow == 2)
                    {
                        TrafficLamp(2, false);
                    }
                    if (bigflow == 1)
                    {
                        TrafficLamp(1, false);
                    }
                    TrafficLamp(3, true);
                }
                else
                {
                    TrafficLamp(1, true);
                    TrafficLamp(2, true);
                }
            }
        }

        public void MoveCars() //0.5 sec
        {

            //Move From Bridge to END

            if (roadQ1.bridgeCars > 0)
            {
                if (carLoc1[2].isCarExists)
                {
                    carLoc1[2].isCarExists = false;
                    roadQ1.bridgeCars--;

                }
                if (carLoc1[1].isCarExists)
                {
                    carLoc1[1].isCarExists = false;
                    carLoc1[2].isCarExists = true;

                }
                if (carLoc1[0].isCarExists)
                {
                    carLoc1[0].isCarExists = false;
                    carLoc1[1].isCarExists = true;

                }
            }



            if (roadQ2.bridgeCars > 0)
            {
                if (carLoc2[2].isCarExists)
                {
                    carLoc2[2].isCarExists = false;
                    roadQ2.bridgeCars--;
                }
                if (carLoc2[1].isCarExists)
                {
                    carLoc2[1].isCarExists = false;
                    carLoc2[2].isCarExists = true;
                }

                if (carLoc2[0].isCarExists)
                {
                    carLoc2[0].isCarExists = false;
                    carLoc2[1].isCarExists = true;
                }


                if (carLoc2[5].isCarExists)
                {
                    carLoc2[5].isCarExists = false;
                    roadQ2.bridgeCars--;
                }
                if (carLoc2[4].isCarExists)
                {
                    carLoc2[4].isCarExists = false;
                    carLoc2[5].isCarExists = true;
                }

                if (carLoc2[3].isCarExists)
                {
                    carLoc2[3].isCarExists = false;
                    carLoc2[4].isCarExists = true;
                }
            }


            if (roadQ3.bridgeCars > 0)
            {
                if (carLoc3[2].isCarExists)
                {
                    carLoc3[2].isCarExists = false;
                    roadQ3.bridgeCars--;

                }
                if (carLoc3[1].isCarExists)
                {
                    carLoc3[1].isCarExists = false;
                    carLoc3[2].isCarExists = true;

                }
                if (carLoc3[0].isCarExists)
                {
                    carLoc3[0].isCarExists = false;
                    carLoc3[1].isCarExists = true;

                }
            }



            //Move From Queue to Bridge
            if (roadQ1.lampStatus == true)
            {
                if (roadQ1.carsWaiting > 0)
                {
                    roadQ1.carsWaiting--;
                    roadQ1.bridgeCars++;
                    carLoc1[0].isCarExists = true;
                }
            }

            if (roadQ2.lampStatus == true)
            {
                if (roadQ2.carsWaiting > 0)
                {
                    roadQ2.carsWaiting--;
                    roadQ2.bridgeCars++;
                    if (roadQ1.lampStatus == true)
                    {
                        carLoc2[0].isCarExists = true;

                    }
                    else if (roadQ3.lampStatus == true)
                    {
                        carLoc2[3].isCarExists = true;
                    }
                }
            }

            if (roadQ3.lampStatus == true)
            {
                if (roadQ3.carsWaiting > 0)
                {
                    roadQ3.carsWaiting--;
                    roadQ3.bridgeCars++;
                    carLoc3[0].isCarExists = true;
                }
            }




        }





    }
}
