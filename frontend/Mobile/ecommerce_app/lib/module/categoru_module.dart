import 'package:ecommerce_app/layout/category_details.dart';
import 'package:ecommerce_app/layout/items_by_category_screen.dart';
import 'package:ecommerce_app/share/app_cubit.dart';
import 'package:ecommerce_app/share/app_state.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_conditional_rendering/conditional.dart';

class CategoryModule extends StatelessWidget
{
  @override
  Widget build(BuildContext context)
  {
    return BlocConsumer<AppCubit, AppStates>(
      listener: (context, state){},
      builder: (context, state)
      {
        AppCubit cubit = AppCubit.get(context);
        if(!cubit.isfinish_categoryModule)
          {
            cubit.getCategoriesData_categoryModule();
          }
        return Conditional.single(
          context: context,
          conditionBuilder: (context) => cubit.isfinish_categoryModule,
          fallbackBuilder: (context) => Center(child: CircularProgressIndicator()),
          widgetBuilder: (context)
            {
              return Padding(
                padding: const EdgeInsets.all(16.0),
                child: Container(
                  child: GridView.count(
                    mainAxisSpacing: 5,
                    crossAxisSpacing: 10,
                    crossAxisCount: 2,
                    children: buildCategoryItem(context, cubit),
                  ),
                ),
              );
            }
        );
      },
    );
  }

  List<Widget> buildCategoryItem(BuildContext context, AppCubit cubit)
  {
    List<Widget> results = [];
    for(int i = 0; i < cubit.categories.length; i++)
    {
      results.add(Container(
        height: 30,
        width: 30,
        decoration: BoxDecoration(
          borderRadius: BorderRadius.circular(12),
          color: Colors.grey[100]
        ),
        child: Column(
          children: [
            Text(
              cubit.categories[i].name!,//cubit.categories[i].name!,
              style: Theme.of(context).textTheme.bodyText1,
            ),
            Expanded(
              child: Image(
                image: NetworkImage(cubit.categories[i].imageUrl!),
              ),
            ),
            Row(
              children: [
                TextButton(
                  onPressed: ()
                  {
                    Navigator.push(context, MaterialPageRoute(builder: (context) => ItemByCategoryScreen(cubit.categories[i], context)));
                  },
                  child: Text(
                      "Items"
                  ),
                ),
                Spacer(),
                TextButton(
                  onPressed: ()
                  {
                    cubit.categorty_categoryDetails = cubit.categories[i];
                    Navigator.push(context, MaterialPageRoute(builder: (context) => CategoryDetails(context)));
                  },
                  child: Text(
                      "Details"
                  ),
                )
              ],
            )
          ],
        ),
      ));
    }
    return results;
  }
}