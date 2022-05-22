import 'dart:io';
import 'package:ecommerce_app/share/app_cubit.dart';
import 'package:ecommerce_app/share/app_state.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:image_picker/image_picker.dart';
import '../share/share_componant.dart';
import '../testing.dart';

class AddCategoryScreen extends StatefulWidget
{
  @override
  State<AddCategoryScreen> createState() => _AddCategoryScreenState();
}


class _AddCategoryScreenState extends State<AddCategoryScreen> {
  GlobalKey<FormState> formKey = GlobalKey();

  TextEditingController textController = TextEditingController();
  TextEditingController descriptionController = TextEditingController();
  TextEditingController attributeController = TextEditingController();

  @override
  Widget build(BuildContext context)
  {

    return BlocConsumer<AppCubit, AppStates>(
      listener: (context, state) {},
      builder: (context,state)
      {
        AppCubit cubit = AppCubit.get(context);
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
                      defaultFormField(
                          controller: descriptionController,
                          type: TextInputType.text,
                          validate: (value)
                          {
                            if(value == null || value.isEmpty)
                            {return "Enter description Name";}
                          },
                          label: "description",
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
                        child: cubit.imageCategory_addCategoryScreen != null? Image.file(cubit.imageCategory_addCategoryScreen!, fit: BoxFit.cover,) : Icon(Icons.camera_alt_outlined),
                      ),
                      SizedBox(height: 15,),
                      Row(
                        children: [
                          Expanded(
                            child: TextFormField(
                              controller: attributeController,
                              keyboardType: TextInputType.text,
                              decoration: InputDecoration(
                                label: Text("Add Attribute"),
                                border: OutlineInputBorder()
                              ),
                            )
                          ),
                          TextButton(
                            onPressed: ()
                            {

                              cubit.categoryAttributes_addCategoryScreen.add(attributeController.text.toString());
                              attributeController.text = "";
                              setState((){});
                            },
                            child: Container(
                              width: 40,
                              height: 40,
                              color: Colors.blue,
                              child: Center(
                                child: Text(
                                  "Add",
                                  style: TextStyle(
                                      color: Colors.white
                                  ),
                                ),
                              ),
                            ),
                          )
                        ],
                      ),
                      SizedBox(height: 15,),
                      Text(
                        "Attribute: ",
                        style: Theme.of(context).textTheme.headline4,
                      ),
                      SizedBox(height: 8,),
                      ListView.separated(
                        shrinkWrap: true,
                        physics: NeverScrollableScrollPhysics(),
                        itemCount: cubit.categoryAttributes_addCategoryScreen.length,
                        itemBuilder: (context, index) => Text(cubit.categoryAttributes_addCategoryScreen[index]),
                        separatorBuilder: (context, index) => SizedBox(height: 8,),
                      ),
                      conditionBuilder(context, (state is AppLoadingState)),
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
            String categoryDescription = descriptionController.text.toString();
            cubit.postImage(cubit.imageCategory_addCategoryScreen!).then((value)
            {
              if(value.statusCode == 200)
              {
                Log.v("Success post image");
                AppCubit.get(context).postNewCategory_addCategoryScreen(catrgoryName, categoryDescription, value.data.toString(), context);
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
              "Create Category",
              style: TextStyle(
                  color: Colors.white
              ),
            ),
          ),
        ),
      );
    }
  }

  void pickImageFromCamera(AppCubit cubit)
  {
    ImagePicker().pickImage(source: ImageSource.camera).then((value)
    {
      if(value!= null)
      {
        Log.v("Success get image");
        setState(() {
          cubit.imageCategory_addCategoryScreen = File(value.path);
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
          cubit.imageCategory_addCategoryScreen = File(value.path);
        });
        Navigator.pop(context);
      }
    }).catchError((e)
    {
      Log.catchE(e);
    });
  }
}

//https://eimagestorage.blob.core.windows.net/images/images/ieuyqhsk.uqw_تنزيل-وتحم.jpeg
// eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhZG1pbkBnbWFpbC5jb20iLCJqdGkiOiJlYTgxYzg3NC1kMjNhLTRkNmMtOGIwOC03MDliOWYwYmNjOWQiLCJlbWFpbCI6ImFkbWluQGdtYWlsLmNvbSIsInVzZXJJZCI6ImM4NzY0NzQ3LWJlYjktNGIwYS05MjllLTg5ZTBlNDk4N2ZjZSIsInBob25lIjoiMDEwNDU4NzY1NDEiLCJwcm9maWxlTmFtZSI6IlRoZSBBZG1pbiIsInJvbGUiOiJBZG1pbiIsIm5iZiI6MTY1Mjk1MTQyNCwiZXhwIjoxNjUyOTU4OTI0LCJpYXQiOjE2NTI5NTE0MjR9
