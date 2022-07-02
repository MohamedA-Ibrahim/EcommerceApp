import 'package:ecommerce_app/share/app_cubit.dart';
import 'package:ecommerce_app/share/app_state.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class CreateOrderScreen extends StatefulWidget
{
  int itemId;
  CreateOrderScreen(this.itemId);
  @override
  State<CreateOrderScreen> createState() => _CreateOrderScreenState(itemId);
}

class _CreateOrderScreenState extends State<CreateOrderScreen>
{
  int itemId;
  _CreateOrderScreenState(this.itemId);
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
                    controller: cubit.phoneNumberController_createOrderScreen,
                    keyboardType: TextInputType.phone,
                    decoration: InputDecoration(
                      label: Text("Phone Number"),
                      border: OutlineInputBorder()
                    ),
                  ),
                  SizedBox(height: 15,),
                  TextFormField(
                    controller: cubit.streetAddressController_createOrderScreen,
                    keyboardType: TextInputType.text,
                    decoration: InputDecoration(
                        label: Text("street Address"),
                        border: OutlineInputBorder()
                    ),
                  ),
                  SizedBox(height: 15,),
                  TextFormField(
                    controller: cubit.cityController_createOrderScreen,
                    keyboardType: TextInputType.text,
                    decoration: InputDecoration(
                        label: Text("City"),
                        border: OutlineInputBorder()
                    ),
                  ),
                  SizedBox(height: 15,),
                  TextFormField(
                    controller: cubit.recieverNameController_createOrderScreen,
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
                      cubit.postOrder_createOrderScreen(
                          context,
                          itemId,
                          cubit.phoneNumberController_createOrderScreen.text.toString(),
                          cubit.streetAddressController_createOrderScreen.text.toString(),
                          cubit.cityController_createOrderScreen.text.toString(),
                          cubit.recieverNameController_createOrderScreen.text.toString()
                      );
                    },
                    child: Container(
                      width: double.infinity,
                      height: 40,
                      color: Colors.blue,
                      child: Center(
                        child: Text(
                          "Order",
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
