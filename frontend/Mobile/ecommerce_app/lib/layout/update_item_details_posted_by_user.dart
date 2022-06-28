import 'package:ecommerce_app/share/app_state.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import '../model/item_model.dart';
import '../share/app_cubit.dart';

class UpdateItemDetailsPostedByUser extends StatelessWidget
{

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
          ),
          body: Padding(
            padding: const EdgeInsets.all(16.0),
            child: SingleChildScrollView(
              child: Column(
                children: [
                  TextFormField(
                    controller: cubit.nameController_updateItemDetailsPostedByUser,
                    keyboardType: TextInputType.text,
                    decoration: InputDecoration(
                        label: Text("Name"),
                        border: OutlineInputBorder()
                    ),
                  ),
                  SizedBox(height: 10,),
                  TextFormField(
                    controller: cubit.descriptionController_updateItemDetailsPostedByUser,
                    keyboardType: TextInputType.text,
                    decoration: InputDecoration(
                        label: Text("Description"),
                        border: OutlineInputBorder()
                    ),
                  ),
                  SizedBox(height: 10,),
                  TextFormField(
                    controller: cubit.priceController_updateItemDetailsPostedByUser,
                    keyboardType: TextInputType.text,
                    decoration: InputDecoration(
                        label: Text("Price"),
                        border: OutlineInputBorder()
                    ),
                  ),
                  SizedBox(height: 10,),
                  ListView.separated(
                    physics: NeverScrollableScrollPhysics(),
                    shrinkWrap: true,
                    itemCount: cubit.attributeValueController_updateItemDetailsPostedByUser.length,
                    separatorBuilder: (context, index) => SizedBox(height: 10,),
                    itemBuilder: (context, index) {
                      return TextFormField(
                        controller: cubit.attributeValueController_updateItemDetailsPostedByUser[index],
                        keyboardType: TextInputType.text,
                        decoration: InputDecoration(
                            label: Text(cubit.attributeValuesItem_updateItemDetailsPostedByUser[index].attributeTypeName),
                            border: OutlineInputBorder()
                        ),
                      );
                    },
                  ),
                  SizedBox(height: 10,),
                  TextButton(
                    onPressed: ()
                    {
                      cubit.updateItemDetails_updateItemsDetailsPostedByUser();
                    },
                    child: Container(
                      height: 50,
                      color: Colors.blue,
                      child: Center(
                        child: Text(
                            "Update",
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
          )
        );
      },
    );
  }
}