import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_conditional_rendering/conditional.dart';

import '../share/app_cubit.dart';
import '../share/app_state.dart';
import '../testing.dart';
import 'item_details_screen.dart';

class SearchItemScreen extends StatefulWidget
{
  @override
  State<SearchItemScreen> createState() => _SearchItemScreenState();
}

class _SearchItemScreenState extends State<SearchItemScreen>
{
  TextEditingController textSearchController = TextEditingController();
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
            title: Text("Search Item"),
          ),
          body: Padding(
            padding: const EdgeInsets.all(16.0),
            child: Column(
              children: [
                TextFormField(
                  onChanged: (value)
                  {
                    cubit.searchItem_searchItemScreen(value);
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
                      itemCount: cubit.item_searchItemScreen.length,
                      separatorBuilder: (context, index) => SizedBox(height: 15,),
                      itemBuilder: (context, index) => GestureDetector(
                        onTap: ()
                        {
                          Log.v(cubit.item_searchItemScreen[index].id!.toString());
                          Log.v(cubit.item_searchItemScreen[index].name!);
                          cubit.getAttributeValues_itemDetaielsScreen(cubit.item_searchItemScreen[index].id!);
                          Navigator.push(context, MaterialPageRoute(builder: (context)=> ItemsDetailsScreen(cubit.item_searchItemScreen[index])));
                        },
                        child: Container(
                          padding: EdgeInsets.all(8),
                          color: Colors.grey[200],
                          width: double.infinity,
                          child: Row(
                            children: [
                              Text(cubit.item_searchItemScreen[index].name!),
                              Spacer(),
                              Image(
                                height: 80,
                                width: 80,
                                image: NetworkImage(cubit.item_searchItemScreen[index].imageUrl!),
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
