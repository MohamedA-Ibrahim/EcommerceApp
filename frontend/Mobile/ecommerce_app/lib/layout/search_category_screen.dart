import 'package:ecommerce_app/share/app_cubit.dart';
import 'package:ecommerce_app/share/app_state.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_conditional_rendering/conditional.dart';

import 'category_details.dart';
import 'items_by_category_screen.dart';

class SearchCategoryScreen extends StatefulWidget
{
  @override
  State<SearchCategoryScreen> createState() => _SearchCategoryScreenState();
}

class _SearchCategoryScreenState extends State<SearchCategoryScreen>
{
  TextEditingController textSearchController = TextEditingController();

  @override
  void initState()
  {
    super.initState();
  }
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
            title: Text("Search Category"),
          ),
          body: Padding(
            padding: const EdgeInsets.all(16.0),
            child: Column(
              children: [
                TextFormField(
                  onChanged: (value)
                  {
                    cubit.searchCategory_searchCategoryScreen(value);
                  },
                  controller: textSearchController,
                  keyboardType: TextInputType.text,
                  decoration: InputDecoration(
                    label: Text("Search"),
                    border: OutlineInputBorder()
                  ),
                ),
                SizedBox(height: 15,),
                Expanded(
                  child: Conditional.single(
                    context: context,
                    conditionBuilder: (context) => !(state is AppLoadingState),
                    fallbackBuilder: (context) => Center(child: CircularProgressIndicator(),),
                    widgetBuilder: (context) => ListView.separated(
                      itemCount: cubit.category_searchCategoryScreen.length,
                      separatorBuilder: (context, index) => SizedBox(height: 15,),
                      itemBuilder: (context, index) => Container(
                        width: double.infinity,
                        child: Row(
                          children: [
                            Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: [
                                Text(cubit.category_searchCategoryScreen[index].name!),
                                Row(
                                  children: [
                                    TextButton(
                                      onPressed: ()
                                      {
                                        Navigator.push(context, MaterialPageRoute(builder: (context) => ItemByCategoryScreen(cubit.category_searchCategoryScreen[index], context)));
                                      },
                                      child: Text("Items"),
                                    ),
                                    SizedBox(width: 15,),
                                    TextButton(
                                      onPressed: ()
                                      {
                                        cubit.categorty_categoryDetails = cubit.category_searchCategoryScreen[index];
                                        Navigator.push(context, MaterialPageRoute(builder: (context) => CategoryDetails(context)));
                                      },
                                      child: Text("Details"),
                                    )
                                  ],
                                )
                              ],
                            ),
                            Spacer(),
                            Image(
                              height: 80,
                              width: 80,
                              image: NetworkImage(cubit.category_searchCategoryScreen[index].imageUrl!),
                            )
                          ],
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
