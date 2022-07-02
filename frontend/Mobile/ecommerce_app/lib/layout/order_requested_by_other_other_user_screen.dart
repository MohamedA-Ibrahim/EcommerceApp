import 'package:ecommerce_app/share/app_cubit.dart';
import 'package:ecommerce_app/share/app_state.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_conditional_rendering/conditional.dart';
import 'package:fluttertoast/fluttertoast.dart';

class OrderRequestedByOtherUserScreen extends StatelessWidget
{
  @override
  Widget build(BuildContext context)
  {
    AppCubit cubit = AppCubit.get(context);
    return BlocConsumer<AppCubit, AppStates>(
      listener: (context, state){},
      builder: (context, state) => Scaffold(
        appBar: AppBar(),
        body: Conditional.single(
          context: context,
          conditionBuilder: (context) => !(state is AppLoadingState),
          fallbackBuilder: (context) => Center(child: CircularProgressIndicator(),),
          widgetBuilder: (context) => ListView.separated(
            itemCount: cubit.requestedOrder_orderRequestedByOtherUserScreen.length,
            separatorBuilder: (context, index) => SizedBox(height: 15,),
            itemBuilder: (context, index) => Padding(
              padding: const EdgeInsets.all(16.0),
              child: Container(
                padding: EdgeInsets.all(10),
                color: Colors.grey,
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      "Buyer UserName: ${cubit.requestedOrder_orderRequestedByOtherUserScreen[index].buyerUserName}"
                    ),
                    SizedBox(height: 15,),
                    Text(
                      "Phone Number: ${cubit.requestedOrder_orderRequestedByOtherUserScreen[index].phoneNumber}"
                    ),
                    SizedBox(height: 15,),
                    Text(
                      "Payment Status: ${cubit.requestedOrder_orderRequestedByOtherUserScreen[index].paymentStatus}"
                    ),
                    SizedBox(height: 15,),
                    Text(
                      "street Address: ${cubit.requestedOrder_orderRequestedByOtherUserScreen[index].streetAddress}"
                    ),
                    SizedBox(height: 15,),
                    Text(
                      "City: ${cubit.requestedOrder_orderRequestedByOtherUserScreen[index].city}"
                    ),
                    SizedBox(height: 15,),
                    Text(
                      "reciever Name: ${cubit.requestedOrder_orderRequestedByOtherUserScreen[index].recieverName}"
                    ),
                    SizedBox(height: 15,),
                    Text(
                        "Order Status: ${cubit.requestedOrder_orderRequestedByOtherUserScreen[index].orderStatus}"
                    ),
                    if(cubit.requestedOrder_orderRequestedByOtherUserScreen[index].orderStatus == "Pending")Row(
                      children: [
                        TextButton(
                          onPressed: ()
                          {
                            cubit.startProcessing_orderRequestedByOtherUser(cubit.requestedOrder_orderRequestedByOtherUserScreen[index].orderId!, cubit.requestedOrder_orderRequestedByOtherUserScreen[index].itemId!);
                          },
                          child: Container(
                            width: 70,
                            height: 40,
                            color: Colors.blue,
                            child: Center(
                              child: Text(
                                "ACCEPT",
                                style: TextStyle(
                                    color: Colors.black
                                ),
                              ),
                            ),
                          ),
                        ),
                        Spacer(),
                        TextButton(
                          onPressed: ()
                          {
                            cubit.rejectOrder_orderRequestedByOtherUser(cubit.requestedOrder_orderRequestedByOtherUserScreen[index].orderId!, cubit.requestedOrder_orderRequestedByOtherUserScreen[index].itemId!);
                          },
                          child: Container(
                            width: 70,
                            height: 40,
                            color: Colors.blue,
                            child: Center(
                              child: Text(
                                "REJECT",
                                style: TextStyle(
                                    color: Colors.black
                                ),
                              ),
                            ),
                          ),
                        ),
                      ],
                    ),
                    if(cubit.requestedOrder_orderRequestedByOtherUserScreen[index].orderStatus == "InProcess")Center(child: TextButton(
                      onPressed: ()
                      {
                        cubit.confirmPayment_orderRequestedByOtherUser(cubit.requestedOrder_orderRequestedByOtherUserScreen[index].orderId!, cubit.requestedOrder_orderRequestedByOtherUserScreen[index].itemId!);
                      },
                      child: Container(
                        width: 150,
                        height: 40,
                        color: Colors.blue,
                        child: Center(
                          child: Text(
                            "Confirm Payment",
                            style: TextStyle(
                                color: Colors.black
                            ),
                          ),
                        ),
                      ),
                    ),),
                    if(cubit.requestedOrder_orderRequestedByOtherUserScreen[index].orderStatus == "Confirmed")Center(child: TextButton(
                      onPressed: ()
                      {
                        cubit.shipOrder_orderRequestedByOtherUser(cubit.requestedOrder_orderRequestedByOtherUserScreen[index].orderId!, cubit.requestedOrder_orderRequestedByOtherUserScreen[index].itemId!);
                      },
                      child: Container(
                        width: 150,
                        height: 40,
                        color: Colors.blue,
                        child: Center(
                          child: Text(
                            "Ship Order",
                            style: TextStyle(
                                color: Colors.black
                            ),
                          ),
                        ),
                      ),
                    ),)
                  ],
                ),
              ),
            ),
          )
        ),
      )
    );
  }
}