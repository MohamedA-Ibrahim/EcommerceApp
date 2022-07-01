import 'package:ecommerce_app/share/app_cubit.dart';
import 'package:ecommerce_app/share/app_state.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class AddressScreen extends StatefulWidget
{
  @override
  State<AddressScreen> createState() => _AddressScreenState();
}

class _AddressScreenState extends State<AddressScreen>
{
  @override
  Widget build(BuildContext context)
  {
    return BlocConsumer<AppCubit, AppStates>(
      listener: (context, state){},
      builder: (context, state)
      {
        AppCubit cubit = AppCubit.get(context);
        return Scaffold(
          appBar: AppBar(
            title: Text("User Address"),
          ),
          body: Padding(
            padding: const EdgeInsets.all(16.0),
            child: SingleChildScrollView(
              child: Column(
                children: [
                  TextFormField(
                    controller: cubit.phoneNumberController_addressScreen,
                    keyboardType: TextInputType.phone,
                    decoration: InputDecoration(
                      label: Text("Phone Number"),
                      border: OutlineInputBorder()
                    ),
                  ),
                  SizedBox(height: 15,),
                  TextFormField(
                    controller: cubit.streetAddressController_addressScreen,
                    keyboardType: TextInputType.text,
                    decoration: InputDecoration(
                        label: Text("street Address"),
                        border: OutlineInputBorder()
                    ),
                  ),
                  SizedBox(height: 15,),
                  TextFormField(
                    controller: cubit.cityController_addressScreen,
                    keyboardType: TextInputType.text,
                    decoration: InputDecoration(
                        label: Text("City"),
                        border: OutlineInputBorder()
                    ),
                  ),
                  SizedBox(height: 15,),
                  TextFormField(
                    controller: cubit.recieverNameController_addressScreen,
                    keyboardType: TextInputType.text,
                    decoration: InputDecoration(
                        label: Text("Reciever Name"),
                        border: OutlineInputBorder()
                    ),
                  ),
                  SizedBox(height: 15,),
                  TextButton(
                    onPressed: ()
                    {
                      cubit.postUserAddress_addressScreen();
                    },
                    child: Container(
                      width: double.infinity,
                      height: 40,
                      color: Colors.blue,
                      child: Center(
                        child: Text(
                          "Update",
                          style: TextStyle(
                            color: Colors.black
                          ),
                        ),
                      ),
                    ),
                  )
                ],
              ),
            ),
          ),
        );
      },
    );
  }
}
