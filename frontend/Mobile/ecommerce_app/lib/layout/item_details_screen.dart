import 'package:ecommerce_app/model/category_model.dart';
import 'package:ecommerce_app/model/item_model.dart';
import 'package:ecommerce_app/share/app_cubit.dart';
import 'package:ecommerce_app/share/app_state.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
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
        return Scaffold(
          appBar: AppBar(
            title: Text(item.name!),
          ),
          body: Padding(
            padding: const EdgeInsets.all(16.0),
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
                    Text(
                      item.description!,
                      style: Theme.of(context).textTheme.bodyText1,
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
                      item.price!.toString(),
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
                Row(
                  children: [
                    Text(
                      "Expiration Date: ",
                      style: Theme.of(context).textTheme.bodyText1,
                    ),
                    SizedBox(width: 25,),
                    Text(
                      item.expirationDate!,
                      style: Theme.of(context).textTheme.bodyText1,
                    ),
                  ],
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
              ],
            ),
          ),
        );
      },
    );
  }
}
