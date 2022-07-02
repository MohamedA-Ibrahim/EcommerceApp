import 'package:ecommerce_app/share/app_cubit.dart';
import 'package:ecommerce_app/share/app_state.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_conditional_rendering/conditional.dart';

class OrderCreatedByLogedUserScreen extends StatelessWidget
{
  @override
  Widget build(BuildContext context)
  {
    AppCubit cubit = AppCubit.get(context);
    return BlocConsumer<AppCubit, AppStates>(
      listener: (context, state){},
      builder: (context, state)
      {
        return Scaffold(
          appBar: AppBar(),
          body: Conditional.single(
            context: context,
            conditionBuilder: (context) => !(state is AppLoadingState),
            fallbackBuilder: (context) => Center(child: CircularProgressIndicator(),),
            widgetBuilder: (context) => Padding(
              padding: const EdgeInsets.all(16.0),
              child: ListView.separated(
                itemCount: cubit.myOrders_orderCreatedByLogedUserScreen.length,
                separatorBuilder: (context, index) => SizedBox(height: 30,),
                itemBuilder: (context, index) => Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      "Item Name: ${cubit.myOrders_orderCreatedByLogedUserScreen[index].itemName}"
                    ),
                    SizedBox(height: 10,),//name
                    Text(
                      "Item Price: ${cubit.myOrders_orderCreatedByLogedUserScreen[index].itemPrice}"
                    ),
                    SizedBox(height: 10,),//price
                    Text(
                      "Seller UserName: ${cubit.myOrders_orderCreatedByLogedUserScreen[index].sellerUserName}"
                    ),
                    SizedBox(height: 10,),//seller user name
                    Text(
                      "Seller Phone Number: ${cubit.myOrders_orderCreatedByLogedUserScreen[index].sellerPhoneNumber}"
                    ),
                    SizedBox(height: 10,),//seller phone number
                    Image(
                      image: cubit.myOrders_orderCreatedByLogedUserScreen[index].itemImageUrl != null ? NetworkImage(cubit.myOrders_orderCreatedByLogedUserScreen[index].itemImageUrl!) : AssetImage("assets/images/error_image.jpg") as ImageProvider,
                      fit: BoxFit.cover,
                    ),
                    SizedBox(height: 10,),
                    Row(
                      children: [
                        Text(
                          "status: ${cubit.myOrders_orderCreatedByLogedUserScreen[index].orderStatus}"
                        ),
                        Spacer(),
                        if(cubit.myOrders_orderCreatedByLogedUserScreen[index].orderStatus == "Pending" ||cubit.myOrders_orderCreatedByLogedUserScreen[index].orderStatus == "InProcess") TextButton(
                          onPressed: ()
                          {
                            cubit.cancelOrder_orderCreatedByLogedUser(cubit.myOrders_orderCreatedByLogedUserScreen[index].id!);
                          },
                          child: Container(
                            width: 70,
                            height: 40,
                            color: Colors.blue,
                            child: Center(
                              child: Text(
                                "Cancel",
                                style: TextStyle(
                                  color: Colors.black
                                ),
                              ),
                            ),
                          ),
                        )
                      ],
                    )//order status + Cansel
                  ],
                ),
              ),
            )
          ),
        );
      },
    );
  }
}