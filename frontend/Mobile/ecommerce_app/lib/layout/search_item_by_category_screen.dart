import 'package:ecommerce_app/share/app_cubit.dart';
import 'package:ecommerce_app/share/app_state.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_conditional_rendering/conditional.dart';

import '../testing.dart';
import 'item_details_screen.dart';

class SearchItemByCategoryScreen extends StatelessWidget
{

  @override
  Widget build(BuildContext context) {
    return BlocConsumer<AppCubit, AppStates>(
      listener: (context, state){},
      builder: (context, state)
      {
        AppCubit cubit = AppCubit.get(context);
        return Scaffold(
          appBar: AppBar(
            leading: IconButton(
              onPressed: ()
              {
                cubit.searchItemsCategoryByName_searchItemsByCategoryScreen.clear();
                Navigator.pop(context);
              },
              icon: Icon(Icons.arrow_back),
            ),
            title: Text("Search Item"),
          ),
          body: Padding(
            padding: const EdgeInsets.all(16.0),
            child: Column(
              children: [
                TextFormField(
                    onChanged: (value)
                    {
                      cubit.searchItemCategoryByName_searchItemCategoryScreen(value);
                    },
                  decoration: InputDecoration(
                    label: Text("Search"),
                    border: OutlineInputBorder()
                ),
                  keyboardType: TextInputType.text,
                ),
                SizedBox(height: 15,),
                Expanded(
                  child: Conditional.single(
                    context: context,
                    conditionBuilder: (context) => !(state is AppLoadingState),
                    fallbackBuilder: (context) => Center(child: CircularProgressIndicator(),),
                    widgetBuilder: (context) => ListView.separated(
                      itemCount: cubit.searchItemsCategoryByName_searchItemsByCategoryScreen.length,
                      separatorBuilder: (context, index) => SizedBox(height: 15,),
                      itemBuilder: (context, index) => GestureDetector(
                        onTap: ()
                        {
                          Log.v(cubit.searchItemsCategoryByName_searchItemsByCategoryScreen[index].id!.toString());
                          Log.v(cubit.searchItemsCategoryByName_searchItemsByCategoryScreen[index].name!);
                          cubit.getAttributeValues_itemDetaielsScreen(cubit.searchItemsCategoryByName_searchItemsByCategoryScreen[index].id!);
                          Navigator.push(context, MaterialPageRoute(builder: (context)=> ItemsDetailsScreen(cubit.searchItemsCategoryByName_searchItemsByCategoryScreen[index])));
                        },
                        child: Container(
                          padding: EdgeInsets.all(8),
                          color: Colors.grey[200],
                          width: double.infinity,
                          child: Row(
                            children: [
                              Text(cubit.searchItemsCategoryByName_searchItemsByCategoryScreen[index].name!),
                              Spacer(),
                              Image(
                                height: 80,
                                width: 80,
                                image: NetworkImage(cubit.searchItemsCategoryByName_searchItemsByCategoryScreen[index].imageUrl!),
                              )
                            ],
                          ),
                        ),
                      ),
                    ),
                  ),
                )
              ],
            ),
          ),
        );
      },
    );
  }
}
