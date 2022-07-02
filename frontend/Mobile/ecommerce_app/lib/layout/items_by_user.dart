import 'package:ecommerce_app/layout/order_requested_by_other_other_user_screen.dart';
import 'package:ecommerce_app/layout/update_item_details_posted_by_user.dart';
import 'package:ecommerce_app/share/app_cubit.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_conditional_rendering/conditional.dart';

import '../share/app_state.dart';
import '../testing.dart';
import 'item_details_screen.dart';

class ItemsByUserScreen extends StatefulWidget
{
  @override
  State<ItemsByUserScreen> createState() => _ItemsByUserScreenState();
}

class _ItemsByUserScreenState extends State<ItemsByUserScreen>
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
          appBar: AppBar(),
          body: Conditional.single(
            context: context,
            conditionBuilder: (context) => !(state is AppLoadingState),
            fallbackBuilder: (context) => Center(child: CircularProgressIndicator(),),
            widgetBuilder: (context) => ListView.separated(
              itemCount: cubit.itemsPostedByUser_itemsByUser.length,
              separatorBuilder: (context, index) => SizedBox(height: 15,),
              itemBuilder: (context, index) => buildItemView(context, index, cubit),
            ),

          ),
        );
      },
    );
  }

  Widget buildItemView(BuildContext context, int index, AppCubit cubit)
  {
    return Padding(
      padding: const EdgeInsets.all(16.0),
      child: GestureDetector(
        onTap: ()
        {
          //Log.v("on tap item in item module item is ${cubit.items[index].name}");
          cubit.itemPostedByUser_updateIOtemDetailsPostedByUser = cubit.itemsPostedByUser_itemsByUser[index];

          //cubit.getAttributeValues_itemDetaielsScreen(cubit.itemsPostedByUser_itemsByUser[index].id!);
          cubit.getAttributeValuesForItems_updateItemDetailsPostedByUser(cubit.itemPostedByUser_updateIOtemDetailsPostedByUser!.id!);
          cubit.nameController_updateItemDetailsPostedByUser.text = cubit.itemPostedByUser_updateIOtemDetailsPostedByUser!.name!;
          cubit.descriptionController_updateItemDetailsPostedByUser.text = cubit.itemPostedByUser_updateIOtemDetailsPostedByUser!.description!;
          cubit.priceController_updateItemDetailsPostedByUser.text = cubit.itemPostedByUser_updateIOtemDetailsPostedByUser!.price!.toString();
          Navigator.push(context, MaterialPageRoute(builder: (context)=> UpdateItemDetailsPostedByUser()));
        },
        child: Container(
          padding: EdgeInsets.all(10),
          width: double.infinity,
          decoration: BoxDecoration(
              color: Colors.grey[100],
              borderRadius: BorderRadius.circular(20)
          ),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                "Name: ${cubit.itemsPostedByUser_itemsByUser[index].name}",
                style: Theme.of(context).textTheme.bodyText1,
              ),
              SizedBox(height: 10,),
              Text(
                "Price: ${cubit.itemsPostedByUser_itemsByUser[index].price}",
                style: Theme.of(context).textTheme.bodyText1,
              ),
              SizedBox(height: 10,),
              Image(
                image: cubit.itemsPostedByUser_itemsByUser[index].imageUrl != null ? NetworkImage(cubit.itemsPostedByUser_itemsByUser[index].imageUrl!) : AssetImage("assets/images/error_image.jpg") as ImageProvider,
                fit: BoxFit.cover,

              ),
              Row(
                children: [
                  TextButton(
                    onPressed: ()
                    {
                      cubit.getAllRequestedOrder_orderRequestedByOtherUser(cubit.itemsPostedByUser_itemsByUser[index].id!);
                      Navigator.push(context, MaterialPageRoute(builder: (context) => OrderRequestedByOtherUserScreen()));
                    },
                    child: Container(
                      width: 70,
                      height: 40,
                      color: Colors.blue,
                      child: Center(
                        child: Text(
                          "ORDERS",
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
                      cubit.deleteItemById_itemsByUser(cubit.itemsPostedByUser_itemsByUser[index].id!);
                    },
                    child: Container(
                      width: 70,
                      height: 40,
                      color: Colors.blue,
                      child: Center(
                        child: Text(
                          "DELETE",
                          style: TextStyle(
                            color: Colors.black
                          ),
                        ),
                      ),
                    ),
                  )
                ],
              )
            ],
          ),
        ),
      ),
    );
  }
}
