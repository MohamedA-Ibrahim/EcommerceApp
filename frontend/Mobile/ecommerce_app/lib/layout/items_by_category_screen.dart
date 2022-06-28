
import 'package:ecommerce_app/layout/search_item_by_category_screen.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_conditional_rendering/conditional.dart';
import '../model/category_model.dart';
import '../share/app_cubit.dart';
import '../share/app_state.dart';
import '../testing.dart';
import 'item_details_screen.dart';

class ItemByCategoryScreen extends StatefulWidget
{
  CategoryModel category;
  BuildContext context;
  ItemByCategoryScreen(this.category, this.context);


  @override
  State<ItemByCategoryScreen> createState() => _ItemByCategoryScreenState(category, context);
}

class _ItemByCategoryScreenState extends State<ItemByCategoryScreen>
{
  @override
  void initState()
  {
    super.initState();
    AppCubit.get(context).getItemsByCategoryId_itemsByCategory(category.id!);
  }
  CategoryModel category;
  BuildContext context;
  _ItemByCategoryScreenState(this.category, this.context);
  @override
  Widget build(BuildContext context)
  {
      return BlocConsumer<AppCubit, AppStates>(
          listener: (context, state)
          {

          },
          builder: (context, state)
          {

            Log.v("Start build Screen ItemByCategoryScreen");
            AppCubit cubit = AppCubit.get(context);
            Log.v("finish get Data");
            return Scaffold(
              appBar: AppBar(
                title: Text(widget.category.name!),
                actions: [
                  IconButton(
                    icon: Icon(Icons.search),
                    onPressed: ()
                    {
                      Navigator.push(context, MaterialPageRoute(builder: (context) => SearchItemByCategoryScreen()));
                    },
                  )
                ],
              ),
              body: ListView.separated(
                itemCount: cubit.itemsByCategoryId.length,
                separatorBuilder: (context, index) => SizedBox(height: 15,),
                itemBuilder: (context, index) => buildItemView(context, index, cubit),
              ),
            );
          }
      );
    }

  Widget buildItemView(BuildContext context, int index, AppCubit cubit)
  {
      return Padding(
        padding: const EdgeInsets.all(16.0),
        child: GestureDetector(
          onTap: ()
          {
            cubit.item_itemDetails = cubit.itemsByCategoryId[index];
            cubit.getAttributeValues_itemDetaielsScreen(cubit.item_itemDetails!.id!);
            Navigator.push(context, MaterialPageRoute(builder: (context)=> ItemsDetailsScreen(cubit.itemsByCategoryId[index])));

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
                  "Name: ${cubit.itemsByCategoryId[index].name}",
                  style: Theme.of(context).textTheme.bodyText1,
                ),
                SizedBox(height: 10,),
                Text(
                  "Price: ${cubit.itemsByCategoryId[index].price}",
                  style: Theme.of(context).textTheme.bodyText1,
                ),
                SizedBox(height: 10,),
                Image(
                  image: cubit.itemsByCategoryId[index].imageUrl != null ? NetworkImage(cubit.itemsByCategoryId[index].imageUrl!) : AssetImage("assets/images/error_image.jpg") as ImageProvider,
                  fit: BoxFit.cover,

                )
              ],
            ),
          ),
        ),
      );
    }
}