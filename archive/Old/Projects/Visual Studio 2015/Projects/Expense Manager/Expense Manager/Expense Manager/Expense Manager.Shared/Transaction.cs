using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Expense_Manager
{
    class Transaction
    {

        App app = (App)Application.Current;
        public int AMOUNT
        {
            get;
            set;
        }

        public string TYPE
        {
            get;
            set;
        }

        public string DATE
        {
            get;
            set;
        }

        public DateTime DATE_OBJ
        {
            get;
            set;
        }

        public string YEAR
        {
            get { return DATE_OBJ.Year.ToString(); }
        }

        public string MONTH
        {
            get { return GetMonthName(DATE_OBJ.Month); }
        }


        public string DAY
        {
            get { return DATE_OBJ.Day.ToString(); }
        }

        public string DESCRIPTION
        {
            get;
            set;
        }

        public char TYPE_IMAGE
        {
            get;
            set;
        }

        public char TRANS_TYPE
        {
            get { return RECIEVER_PERS == "SELF" ? 'i' : 'e'; }
            set { }
        }


        public string RECIEVER_PERS
        {
            get;
            set;
        }

        public ImageBrush RECIEVER_IMG
        {
            get;
            set;
        }
        

        public Transaction(int amt, string type, char typeImage, DateTime date, string reciever, ImageBrush recv_img, string description)
        {
            this.AMOUNT = amt;
            this.TYPE = type;
            this.TYPE_IMAGE = typeImage;
            this.DATE_OBJ = date;
            this.DATE = date.Day + "/" + date.Month + "/" + date.Year;
            this.DESCRIPTION = description;
            if (recv_img == null) this.RECIEVER_IMG = new ImageBrush() { Stretch = Stretch.Uniform, ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/Images/acc_boy.jpg", UriKind.Absolute)) }; //add "no user" image if recieved imageBrush is null
            else this.RECIEVER_IMG = recv_img;
            if ((this.RECIEVER_PERS = reciever) == "SELF")
                app.TotalIncome += amt;
            else app.TotalExpense += amt;
        }


        private string GetMonthName(int month)
        {
            switch (month)
            {
                case 1:
                    return "JAN";

                case 2:
                    return "FEB";
                case 3:
                    return "MAR";
                case 4:
                    return "APR";
                case 5:
                    return "MAY";
                case 6:
                    return "JUN";
                case 7:
                    return "JUL";
                case 8:
                    return "AUG";
                case 9:
                    return "SEP";
                case 10:
                    return "OCT";
                case 11:
                    return "NOV";
                case 12:
                    return "DEC";
                default:
                    return "null";


            }
        }
    }
}
