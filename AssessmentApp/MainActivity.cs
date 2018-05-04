using Android.App;
using Android.Widget;
using Android.OS;
using AssessmentApp.model;
using System.Collections.Generic;
using AssessmentApp.adapter;
using AssessmentApp;
using AssessmentApp.sqlite;
using System;
using AssessmentApp.hepler;
using Android.Util;
using Java.Lang;

namespace AssessmentApp
{
    [Activity(Label = "My Customers", MainLauncher = true)]
    public class MainActivity : Activity
    {
        List<Customer> customerList;
        CustomerAdapter customerAdapter;
        Database db;
        ListView listViewCustomers;
        EditText editTextFirstName;
        EditText editTextLastName;
        EditText editTextAge;
        EditText editTextIDNumber;
        Button buttonSave, buttonUpdate, buttonDelete;
        private const int DELETE = 0,UPDATE = 1;
        private int status;
        Validator inputValidator;
        AdapterView.ItemClickEventArgs e;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            inputValidator = new Validator(this);
            db = new Database();
            db.createDatabase();

            listViewCustomers = FindViewById<ListView>(Resource.Id.listViewCustomers);

            editTextFirstName = FindViewById<EditText>(Resource.Id.editTextFirstName);
            editTextLastName = FindViewById<EditText>(Resource.Id.editTextLastName);
            editTextAge = FindViewById<EditText>(Resource.Id.editTextAge);
            editTextAge.Enabled = false;
            editTextIDNumber = FindViewById<EditText>(Resource.Id.editTextIDNumber);
            buttonSave = FindViewById<Button>(Resource.Id.buttonSave);
            buttonUpdate = FindViewById<Button>(Resource.Id.buttonUpdate);
            buttonDelete = FindViewById<Button>(Resource.Id.buttonDelete);
            customerList = new List<Customer>();
            customerAdapter = new CustomerAdapter(customerList);
            LoadCustomerData();

            buttonSave.Click += delegate
            {
                AddCustomer();
            };

            buttonUpdate.Click += delegate
            {
                status = 1;
                UpdateRemoveCustomer();

            };

            buttonDelete.Click += delegate
            {
                status = 0;
                UpdateRemoveCustomer();

            };

        }

        private void AddCustomer()
        {
            bool isInputEmpty = this.isInputEmpty();

            if (!isInputEmpty)
            {
                IDValidator validator = new IDValidator(editTextIDNumber.Text);
                Customer customer = new Customer()
                {

                    Firstname = editTextFirstName.Text,
                    Lastname = editTextLastName.Text,
                    Idnumber = editTextIDNumber.Text,
                    Age = validator.GetAge()

                };

                bool exist = inputValidator.validateInputIDNumber(editTextIDNumber.Text, customerList);
                if (exist)
                {
                    db.insertIntoTable(customer);
                    LoadCustomerData();
                    ClearImputValues();
                }
            }
        }

        private void UpdateRemoveCustomer()
        {
            bool isInputEmpty = this.isInputEmpty();
            if (!isInputEmpty) { 
            IDValidator validator = new IDValidator(editTextIDNumber.Text);
            Customer customer = new Customer()
            {
                Id = int.Parse(editTextFirstName.Tag.ToString()),
                Firstname = editTextFirstName.Text,
                Lastname = editTextLastName.Text,
                Idnumber = editTextIDNumber.Text,
                Age = validator.GetAge()

            };

            switch (status)
            {
                case DELETE:
                    DeleteCustomerconfirmDialog(customer);
                    break;
                case UPDATE:
                    bool exist = inputValidator.validateInputIDNumberUpdate(editTextIDNumber.Text);
                    if (exist)
                    {
                            if (db.updateTable(customer))
                            {
                                toast("Customer is updated successfully");
                                LoadCustomerData();
                            }
                            else
                            {
                                toast("Customer is NOT updated");
                            }

                    }
                
                   
                    break;
            }
            }

        }

        private void LoadCustomerData()
        {
  
            customerList = db.selectCustomers();

            customerAdapter = new CustomerAdapter(customerList);
            listViewCustomers.Adapter = customerAdapter;

            ViewCustomer();

        }

        private void ViewCustomer()
        {
            listViewCustomers.ItemClick += (s, e) =>
            {
                this.e = e;
                for (int i = 0; i < listViewCustomers.Count; i++)
                {
                    if (e.Position == i)
                        listViewCustomers.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.Gray);
                    else
                        listViewCustomers.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.Transparent);
                }

                editTextFirstName.Text = customerList[e.Position].Firstname;
                editTextLastName.Text = customerList[e.Position].Lastname;
                editTextIDNumber.Text = customerList[e.Position].Idnumber;
                editTextFirstName.Tag = customerList[e.Position].Id;
                editTextAge.Text = customerList[e.Position].Age.ToString();
            };
        }

        private void ClearImputValues()
        {
            editTextFirstName.Text = "";
            editTextLastName.Text = "";
            editTextIDNumber.Text = "";
            editTextAge.Text = "";
        }

        public void DeleteCustomerconfirmDialog(Customer customer)
        {

            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            alert.SetTitle("Confirm Delete");
            alert.SetMessage("Delete "+ customer.Firstname + " " +customer.Lastname);
            alert.SetPositiveButton("Yes", (senderAlert, args) => {
                if (db.removeTable(customer))
                {
                    toast("Customer is removed");
                    LoadCustomerData();
                }
                else
                {
                    toast("Customer is NOT removed");
                }
               
            });

            alert.SetNegativeButton("No", (senderAlert, args) => {
               
            });

            Dialog dialog = alert.Create();
            dialog.Show();

        }

        private bool isInputEmpty()
        {
            bool fname =  inputValidator.validateImput(editTextFirstName);
            bool lname  = inputValidator.validateImput(editTextLastName);
            bool idno =  inputValidator.validateImput(editTextIDNumber);

            if(fname || lname || idno)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void toast(string message)
        {
            Toast.MakeText(this, message, ToastLength.Short).Show();
        }

      
    }

    


}

