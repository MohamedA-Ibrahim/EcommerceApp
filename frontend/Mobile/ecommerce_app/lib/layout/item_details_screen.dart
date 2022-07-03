import 'package:ecommerce_app/layout/create_order_screen.dart';
import 'package:ecommerce_app/model/category_model.dart';
import 'package:ecommerce_app/model/item_model.dart';
import 'package:ecommerce_app/share/app_cubit.dart';
import 'package:ecommerce_app/share/app_state.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import '../testing.dart';
class ItemsDetailsScreen extends StatefulWidget
{
  ItemModel item;
  ItemsDetailsScreen(this.item);
  @override
  State<ItemsDetailsScreen> createState() => _ItemsDetailsScreenState(item);
}

class _ItemsDetailsScreenState extends State<ItemsDetailsScreen>
{

  ItemModel item;
  _ItemsDetailsScreenState(this.item);
  @override
  void initState()
  {
    super.initState();
  }
  @override
  Widget build(BuildContext context)
  {
    return BlocConsumer<AppCubit, AppStates>(
      listener: (context, state) {},
      builder: (context, state)
      {
        AppCubit cubit = AppCubit.get(context);
        return Scaffold(
          appBar: AppBar(
            title: Text(item.name!),
          ),
          body: Padding(
            padding: const EdgeInsets.all(16.0),
            child: SingleChildScrollView(
              child: Column(
                children: [
                  Row(
                    children: [
                      Text(
                        "Name: ",
                        style: Theme.of(context).textTheme.bodyText1,
                      ),
                      SizedBox(width: 25,),
                      Text(
                        item.name!,
                        style: Theme.of(context).textTheme.bodyText1,
                      ),
                    ],
                  ),
                  SizedBox(height: 15,),
                  Row(
                    children: [
                      Text(
                        "description: ",
                        style: Theme.of(context).textTheme.bodyText1,
                      ),
                      SizedBox(width: 25,),
                      Expanded(
                        child: Container(
                          child: Text(
                            item.description!,
                            style: Theme.of(context).textTheme.bodyText1,
                          ),
                        ),
                      ),
                    ],
                  ),
                  SizedBox(height: 15,),
                  Row(
                    children: [
                      Text(
                        "Price: ",
                        style: Theme.of(context).textTheme.bodyText1,
                      ),
                      SizedBox(width: 25,),
                      Text(
                        item.price.toString(),
                        style: Theme.of(context).textTheme.bodyText1,
                      ),
                    ],
                  ),
                  SizedBox(height: 15,),
                  Row(
                    children: [
                      Text(
                        "category: ",
                        style: Theme.of(context).textTheme.bodyText1,
                      ),
                      SizedBox(width: 25,),
                      Text(
                        item.category!.name!,
                        style: Theme.of(context).textTheme.bodyText1,
                      ),
                    ],
                  ),
                  SizedBox(height: 15,),
                  GridView.count(
                    mainAxisSpacing: 5,
                    crossAxisSpacing: 10,
                    crossAxisCount: 2,
                    childAspectRatio: 3,
                    shrinkWrap: true,
                    physics: NeverScrollableScrollPhysics(),
                    children: List.generate(
                        cubit.itemAttributesValues_itemsDetailsScreen != null ? cubit.itemAttributesValues_itemsDetailsScreen!.data.length : 0,
                            (index) => Container(
                          color: Colors.blue,
                          child: Center(
                            child: Text(
                              "${cubit.itemAttributesValues_itemsDetailsScreen!.data[index]["attributeType"]["name"]}:${cubit.itemAttributesValues_itemsDetailsScreen!.data[index]["value"]}",
                              style: Theme.of(context).textTheme.bodyText2,
                            ),
                          ),
                        )
                    ),
                  ),
                  SizedBox(height: 15,),
                  Container(
                    width: 300,
                    height: 300,
                    child: item.imageUrl != null? Image(
                      image: NetworkImage(item.imageUrl!),
                      fit: BoxFit.contain,
                    ) : Image(
                      image: AssetImage("assets/images/error_image.jpg"),
                    ),
                  ),
                  SizedBox(height: 15,),
                  TextButton(
                    onPressed: ()
                    {
                      cubit.getUserAddress_userAddressScreen();
                      cubit.phoneNumberController_createOrderScreen.text = cubit.userAddress_userAddressScreen!.phoneNumber;
                      cubit.recieverNameController_createOrderScreen.text = cubit.userAddress_userAddressScreen!.recieverName;
                      cubit.cityController_createOrderScreen.text = cubit.userAddress_userAddressScreen!.city;
                      cubit.streetAddressController_createOrderScreen.text = cubit.userAddress_userAddressScreen!.streetAddress;
                      Navigator.push(context, MaterialPageRoute(builder: (context) => CreateOrderScreen(item.id!)));
                    },
                    child: Container(
                      height: 50,
                      color: Colors.blue,
                      child: Center(
                        child: Text(
                          "BUY",
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
