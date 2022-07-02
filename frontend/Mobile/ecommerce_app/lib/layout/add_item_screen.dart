import 'dart:io';
import 'package:ecommerce_app/model/category_model.dart';
import 'package:ecommerce_app/share/app_state.dart';
import 'package:ecommerce_app/share/share_componant.dart';
import 'package:ecommerce_app/testing.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:image_picker/image_picker.dart';
import 'package:intl/intl.dart';

import '../share/app_cubit.dart';

class AddItemScreen extends StatefulWidget
{
  @override
  State<AddItemScreen> createState() => _AddItemScreenState();
}

class _AddItemScreenState extends State<AddItemScreen> {

GlobalKey<FormState> formKey = GlobalKey();
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
          body: SingleChildScrollView(
            child: Form(
              key: formKey,
              child: Padding(
                padding: const EdgeInsets.all(16.0),
                child: Column(
                  children: [
                    //name
                    defaultFormField(
                        controller: cubit.name_addItemScreen,
                        type: TextInputType.text,
                        validate: (value)
                        {
                          if(value == null || value.isEmpty)
                            {
                              return "Enter name of item";
                            }
                        },
                        label: "Name",
                        ),
                    SizedBox(height: 15,),
                    //discription
                    defaultFormField(
                        controller: cubit.discription_addItemScreen,
                        type: TextInputType.text,
                        validate: (value)
                        {
                          if(value == null || value.isEmpty)
                          {
                            return "Enter description of item";
                          }
                        },
                        label: "Description",
                        ),
                    SizedBox(height: 15,),

                    //price
                    defaultFormField(
                        controller: cubit.price_addItemScreen,
                        type: TextInputType.number,
                        validate: (value)
                        {
                          if(value == null || value.isEmpty)
                          {
                            return "Enter Price of item";
                          }
                        },
                        label: "Price",
                        ),
                    SizedBox(height: 15,),
                    TextButton(
                      onPressed: ()
                      {
                        showModalBottomSheet(context: context, builder: (context)
                        {
                          return SafeArea(
                            child: Wrap(
                              children: [
                                ListTile(
                                  leading: Icon(Icons.browse_gallery_outlined),
                                  title: Text("Gallery"),
                                  onTap: ()
                                  {
                                    pickImageFromGallery(cubit);
                                  },
                                ),
                                ListTile(
                                  leading: Icon(Icons.camera_alt_outlined),
                                  title: Text("Camera"),
                                  onTap: ()
                                  {
                                    pickImageFromCamera(cubit);
                                  },
                                )
                              ],
                            ),
                          );
                        });
                      },
                      child: Text("Upload Image"),
                    ),
                    SizedBox(height: 15,),
                    Container(
                      width: 300,
                      height: 300,
                      color: Colors.black26,
                      child: cubit.image_addItemScreen != null? Image.file(cubit.image_addItemScreen!, fit: BoxFit.cover,) : Icon(Icons.camera_alt_outlined),
                    ),
                    SizedBox(height: 15,),
                    Row(
                      children: [
                        TextButton(
                          onPressed: ()
                          {
                            showModalBottomSheet(context: context, builder: (context)
                            {
                              return SafeArea(
                                  child: SingleChildScrollView(
                                    child: Wrap(
                                      children: bulidListCategory(context, cubit)
                                    ),
                                  )
                              );
                            });
                          },
                          child: Text("Chose Category"),
                        ),
                        Text(
                            cubit.categoryForItem_addItemScreen == null ? "" : cubit.categoryForItem_addItemScreen!.name!
                        )
                      ],
                    ),
                    SizedBox(height: 15,),
                    ListView.separated(
                      shrinkWrap: true,
                      physics: NeverScrollableScrollPhysics(),
                      itemCount: cubit.attributeValuesControllers_addItemScreen.length,
                      separatorBuilder: (context, index) => SizedBox(height: 10,),
                      itemBuilder: (context, index) => TextFormField(
                        validator: (value)
                        {
                          if(value == null || value.isEmpty)
                            {
                              return "Please Enter Value of Attribute";
                            }
                        },
                        controller: cubit.attributeValuesControllers_addItemScreen[index],
                        keyboardType: TextInputType.text,
                        decoration: InputDecoration(
                            label: Text(cubit.attributeTypeByCategoryId_addItemScreen[index]["name"]),
                            border: OutlineInputBorder()
                        ),
                      ),
                    ),
                    SizedBox(height: 15,),
                    conditionBuilder(context, (state is AppLoadingState), cubit)
                  ],
                ),
              ),
            ),
          )
        );
      },
    );
  }

  void pickImageFromCamera(AppCubit cubit)
  {
    ImagePicker().pickImage(source: ImageSource.camera).then((value)
    {
      if(value!= null)
        {
          Log.v("Success get image");
          setState(() {
            cubit.image_addItemScreen = File(value.path);
          });
          Navigator.pop(context);
        }
    }).catchError((e)
    {
      Log.catchE(e);
    });
  }

  void pickImageFromGallery(AppCubit cubit)
  {
    ImagePicker().pickImage(source: ImageSource.gallery).then((value)
    {
      if(value!= null)
      {
        Log.v("Success get image");
        setState(() {
          cubit.image_addItemScreen = File(value.path);
        });
        Navigator.pop(context);
      }
    }).catchError((e)
    {
      Log.catchE(e);
    });
  }

  List<Widget> bulidListCategory(BuildContext context, AppCubit cubit)
  {
    List<Widget> result = [];
    Log.v(cubit.categories.length.toString());
    for(int i = 0; i < cubit.categories.length; i++)
      {
        result.add(ListTile(
          title: Text(cubit.categories[i].name!),
          onTap: ()
          {
            cubit.categoryForItem_addItemScreen = cubit.categories[i];

            setState(() {
              cubit.getAttributeTypeByCategoryId_addItemScreen(cubit.categoryForItem_addItemScreen!.id!);
            });
            Navigator.pop(context);
          },
        ));
      }
    return result;
  }
  Widget conditionBuilder(BuildContext context, bool flag, AppCubit cubit)
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
          cubit.postImage(cubit.image_addItemScreen!).then((value)
          {
            if(value.statusCode == 200)
              {
                Log.v("Success post image");
                cubit.postNewItem_addItemScreen(context, cubit.name_addItemScreen.text.toString(), cubit.discription_addItemScreen.text.toString(), int.parse(cubit.price_addItemScreen.text.toString()), value.data.toString(), cubit.categoryForItem_addItemScreen!.id!);
              }
          }).catchError((e)
          {
            Log.catchE(e);
            Log.e("Error on AddItemScreen.conditionBuilder 259");
          });
        }
      },
      child: Container(
        width: double.infinity,
        height: 40,
        color: Colors.blue,
        child: Center(
          child: Text(
            "Create Item",
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