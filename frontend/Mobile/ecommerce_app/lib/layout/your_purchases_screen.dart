import 'package:ecommerce_app/share/app_cubit.dart';
import 'package:ecommerce_app/share/app_state.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_conditional_rendering/conditional.dart';

class YourPurchasesScreen extends StatefulWidget
{
  @override
  State<YourPurchasesScreen> createState() => _YourPurchasesScreenState();
}

class _YourPurchasesScreenState extends State<YourPurchasesScreen>
{
  @override
  Widget build(BuildContext context) {
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
            widgetBuilder: (context) => Padding(
              padding: const EdgeInsets.all(8.0),
              child: ListView.separated(
                itemCount: cubit.itemsBroughtByMeModel!.itemsBroughtByMe.length,
                separatorBuilder: (context, index) => SizedBox(height: 15,),
                itemBuilder: (context, index) => Container(
                  color: Colors.grey,
                  padding: EdgeInsets.all(16),
                  child: Row(
                    children: [
                      Text(cubit.itemsBroughtByMeModel!.itemsBroughtByMe[index].itemName!),
                      Spacer(),
                      Text(cubit.itemsBroughtByMeModel!.itemsBroughtByMe[index].orderStatus!)
                    ],
                  ),
                ),
              ),
            ),
          )
        );
      },
    );
  }
}
