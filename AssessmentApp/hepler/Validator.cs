using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AssessmentApp.model;

namespace AssessmentApp.hepler
{
    class Validator
    {
        private Context context;

        public Validator(Context context)
        {
            this.context = context;
        }

        /// <summary>
        /// check for empty iput
        /// </summary>
        /// <param name="editText"></param>
        /// <returns></returns>
 
        public bool validateImput(EditText editText)
        {
            bool isInputEmpty  = String.IsNullOrEmpty(editText.Text) ? true:false;
           
            if (isInputEmpty)
            {
                editText.Error = "Input is required";
                return true;
            }
            else
            {
                return false;
            }
           
        }


        /// <summary>
        /// check if the customer already exist 
        /// </summary>
        /// <param name="customerList"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool userExist(List<Customer> customerList,string id)
        {
            bool exist = false;
            for(int i=0;i < customerList.Count; i++)
            {
                if(string.Compare(id,customerList[i].Idnumber) == 0){
                    exist = true;
                    break;
                }
                else
                {
                    exist = false;
                }
            }

            return exist;


        }

        /// <summary>
        ///
        /// check if ID number is valid 
        /// and the customer's age is 18 or older
        /// Retues true if id is valid and age=>18 else return false
        /// used userExist to check if the customer already exist 
        /// </summary>
        /// <param name="IDNo"></param>
        /// <param name="customerList"></param>
        /// <returns></returns>
        public bool validateInputIDNumber(string IDNo, List<Customer> customerList)
        {

            IDValidator validator = new IDValidator(IDNo);

            if (validator.isValid())
            {
                if (validator.GetAge() > 18)
                {
                    if (!userExist(customerList, IDNo))
                    {
                        return true;
                    }
                    else
                    {
                        toast("Customer already exist");
                        return false;
                    }
                }
                else
                {
                    toast("Age mus be 18 or older");
                    return false;
                }
            }
            else
            {
                toast("Invalid Id number");
                return false;
            }
        }

        /// <summary>
        /// This methord is used when updating the user
        /// </summary>
        /// <param name="IDNo"></param>
        /// <returns></returns>
        public bool validateInputIDNumberUpdate(string IDNo)
        {

            IDValidator validator = new IDValidator(IDNo);

            if (validator.isValid())
            {
                if (validator.GetAge() > 18)
                {
                    return true;
                }
                else
                {
                    toast("Age mus be 18 or older");
                    return false;
                }
            }
            else
            {
                toast("Invalid Id number");
                return false;
            }
        }

        /// <summary>
        /// Custom toast message
        /// </summary>
        /// <param name="message"></param>
        private void toast(string message)
        {
            Toast.MakeText(context, message, ToastLength.Short).Show();
        }

    }
}