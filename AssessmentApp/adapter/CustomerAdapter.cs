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

namespace AssessmentApp.adapter
{
    class CustomerAdapter : BaseAdapter<Customer>
    {
        List<Customer> mCustomerList;

        public CustomerAdapter(List<Customer> mCustomerList)
        {
            this.mCustomerList = mCustomerList;
        }

        public override int Count
        {
            get { return mCustomerList.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;

            if (view == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.customer_item, parent, false);

                var textViewFirstname = view.FindViewById<TextView>(Resource.Id.textViewFirstname);
                var textViewLastname = view.FindViewById<TextView>(Resource.Id.textViewLastname);
                var idnumber = view.FindViewById<TextView>(Resource.Id.textViewIdNumer);
                var age = view.FindViewById<TextView>(Resource.Id.textViewAge);

                view.Tag = new ViewHolder() {
                    TextViewFirstName = textViewFirstname,
                    TextViewLastName = textViewLastname,
                    TextViewIdNumer = idnumber,
                    TextViewAge = age
                };
            }

            var holder = (ViewHolder)view.Tag;

            Customer customer = mCustomerList[position];

            holder.TextViewFirstName.Text = customer.Firstname;
            holder.TextViewLastName.Text = customer.Lastname;
            holder.TextViewIdNumer.Text = customer.Idnumber;
            holder.TextViewAge.Text = customer.Age.ToString();

            return view;
        }

        public override long GetItemId(int position)
        {

            return position;
        }

        public override Customer this[int position]
        {
            get
            {
                return mCustomerList[position];
            }
        }

        class ViewHolder : Java.Lang.Object
        {
            public TextView TextViewFirstName { get; set; }
            public TextView TextViewLastName { get; set; }
            public TextView TextViewIdNumer { get; set; }
            public TextView TextViewAge { get; set; }
        }
    }
}