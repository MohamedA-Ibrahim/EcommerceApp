import 'package:ecommerce_app/share/app_cubit.dart';
import 'package:ecommerce_app/share/app_state.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import '../share/share_componant.dart';

class AddCategoryScreen extends StatelessWidget
{
  GlobalKey<FormState> formKey = GlobalKey();
  TextEditingController textController = TextEditingController();
  @override
  Widget build(BuildContext context)
  {

    return BlocConsumer<AppCubit, AppStates>(
      listener: (context, state) {},
      builder: (context,state)
      {
        return Scaffold(
          appBar: AppBar(),
          body: Center(
            child: SingleChildScrollView(
              child: Padding(
                padding: const EdgeInsets.all(10.0),
                child: Form(
                  key: formKey,
                  child: Column(
                    children: [
                      defaultFormField(
                          controller: textController,
                          type: TextInputType.text,
                          validate: (value)
                          {
                            if(value == null || value.isEmpty)
                              {return "Enter Category Name";}
                          },
                          label: "Category Name",
                          prefix: Icons.category_rounded
                      ),
                      SizedBox(height: 15,),
                      conditionBuilder(context, (state is AppLoadingState))
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

  Widget conditionBuilder(BuildContext context, bool flag)
  {
    if(flag)
    {
      return Center(child: CircularProgressIndicator(),);
    }
    else
    {
      return TextButton(
        onPressed: ()
        {
          AppCubit cubit = AppCubit.get(context);
          if(formKey.currentState!.validate())
          {
            String catrgoryName = textController.text.toString();
            AppCubit.get(context).postNewCategory_addCategoryScreen(catrgoryName, context);
          }
        },
        child: Container(
          width: double.infinity,
          height: 40,
          color: Colors.blue,
          child: Center(
            child: Text(
              "ADD",
              style: TextStyle(
                  color: Colors.white
              ),
            ),
          ),
        ),
      );
    }
  }

}