import 'package:ecommerce_app/layout/item_details_screen.dart';
import 'package:ecommerce_app/share/app_cubit.dart';
import 'package:ecommerce_app/share/app_state.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_conditional_rendering/conditional.dart';

import '../testing.dart';

class ItemModule extends StatelessWidget
{
  @override
  Widget build(BuildContext context)
  {
    return BlocConsumer<AppCubit, AppStates>(
      listener: (context, state)
      {

      },
      builder: (context, state)
      {
        if(!AppCubit.get(context).isfinish_itemsModule)
        {
          AppCubit.get(context).getItemsData_itemModule();
        }
        AppCubit cubit = AppCubit.get(context);
        return Conditional.single(
          context: context,
          conditionBuilder: (context) => cubit.isfinish_itemsModule,
            fallbackBuilder: (context) => Center(child: CircularProgressIndicator(),),
          widgetBuilder: (context) => ListView.separated(
            itemCount: cubit.items.length,
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
          Log.v("on tap item in item module item is ${cubit.items[index].name}");
          cubit.item_itemDetails = cubit.items[index];
          cubit.getAttributeValues_itemDetaielsScreen(cubit.item_itemDetails!.id!);
          Navigator.push(context, MaterialPageRoute(builder: (context)=> ItemsDetailsScreen(cubit.items[index])));
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
                "Name: ${cubit.items[index].name}",
              style: Theme.of(context).textTheme.bodyText1,
            ),
            SizedBox(height: 10,),
            Text(
                "Price: ${cubit.items[index].price}",
              style: Theme.of(context).textTheme.bodyText1,
            ),
            SizedBox(height: 10,),
            Image(
              image: cubit.items[index].imageUrl != null ? NetworkImage(cubit.items[index].imageUrl!) : AssetImage("assets/images/error_image.jpg") as ImageProvider,
              fit: BoxFit.cover,
            )
          ],
          ),
        ),
      ),
    );
  }
}