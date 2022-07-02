import 'package:ecommerce_app/share/app_cubit.dart';
import 'package:ecommerce_app/share/app_state.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class UserAddressScreen extends StatelessWidget
{
  @override
  Widget build(BuildContext context)
  {
    AppCubit cubit = AppCubit.get(context);
    return BlocConsumer<AppCubit, AppStates>(
      listener: (context, state){},
      builder: (context, state){
        return Scaffold(
          appBar: AppBar(),
          body: Padding(
            padding: const EdgeInsets.all(16.0),
            child: SingleChildScrollView(
              child: Column(
                children: [
                  TextFormField(
                    controller: cubit.phoneNumberController_userAddressScreen,
                    keyboardType: TextInputType.phone,
                    decoration: InputDecoration(
                        label: Text("Phone Number"),
                        border: OutlineInputBorder()
                    ),
                  ),
                  SizedBox(height: 15,),
                  TextFormField(
                    controller: cubit.streetAddressController_userAddressScreen,
                    keyboardType: TextInputType.text,
                    decoration: InputDecoration(
                        label: Text("street Address"),
                        border: OutlineInputBorder()
                    ),
                  ),
                  SizedBox(height: 15,),
                  TextFormField(
                    controller: cubit.cityController_userAddressScreen,
                    keyboardType: TextInputType.text,
                    decoration: InputDecoration(
                        label: Text("City"),
                        border: OutlineInputBorder()
                    ),
                  ),
                  SizedBox(height: 15,),
                  TextFormField(
                    controller: cubit.recieverNameController_userAddressScreen,
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
                      cubit.postUserAddress_userAddressScreen(
                          cubit.phoneNumberController_userAddressScreen.text.toString(),
                          cubit.streetAddressController_userAddressScreen.text.toString(),
                          cubit.cityController_userAddressScreen.text.toString(),
                          cubit.recieverNameController_userAddressScreen.text.toString()
                      );
                    },
                    child: Container(
                      width: double.infinity,
                      height: 40,
                      color: Colors.blue,
                      child: Center(
                        child: Text(
                          "UPDATE",
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