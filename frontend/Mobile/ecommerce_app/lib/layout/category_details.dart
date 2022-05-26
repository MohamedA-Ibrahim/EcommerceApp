import 'package:ecommerce_app/share/app_cubit.dart';
import 'package:ecommerce_app/share/app_state.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class CategoryDetails extends StatefulWidget
{
  BuildContext context;
  CategoryDetails(this.context);
  @override
  State<CategoryDetails> createState() => _CategoryDetailsState(context);
}

class _CategoryDetailsState extends State<CategoryDetails>
{
  BuildContext context;
  _CategoryDetailsState(this.context);

  @override
  void initState()
  {
   AppCubit.get(context).getAttributeType_categoryDetails();
  }

  @override
  Widget build(BuildContext context)
  {
    var size = MediaQuery.of(context).size;
    double itemHeight = (size.height - kToolbarHeight - 24) / 2;
    double itemWidth = size.width / 2;
    AppCubit cubit = AppCubit.get(context);
    return BlocConsumer<AppCubit, AppStates>(
      listener: (context, state){},
      builder: (context, state)
      {
        return Scaffold(
          appBar: AppBar(
            title: Text(cubit.categorty_categoryDetails!.name!),
          ),
          body: SingleChildScrollView(
            child: Container(
              padding: EdgeInsets.all(16),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Center(
                    child: Container(
                      width: 350,
                      height: 350,
                      child: Image(
                        image: NetworkImage(cubit.categorty_categoryDetails!.imageUrl!),
                      ),
                    ),
                  ),
                  SizedBox(height: 15,),
                  Text(cubit.categorty_categoryDetails!.description!),
                  SizedBox(height: 20,),
                  Text(
                      "Attribute: ",
                    style: Theme.of(context).textTheme.headline4,
                  ),
                  SizedBox(height: 10,),
                  GridView.count(
                    mainAxisSpacing: 5,
                    crossAxisSpacing: 10,
                    crossAxisCount: 2,
                    childAspectRatio: 3,
                    shrinkWrap: true,
                    physics: NeverScrollableScrollPhysics(),
                    children: List.generate(
                        cubit.attributeType_categoryDetails.length,
                            (index) => Container(
                              color: Colors.blue,
                              child: Center(
                                child: Text(
                                  cubit.attributeType_categoryDetails[index],
                                  style: Theme.of(context).textTheme.bodyText2,
                                ),
                              ),
                            )
                    ),
                  ),
                  buildDeleteCategory(context, (cubit.user!.role == "Admin"))
                ],
              ),
            ),
          ),
        );
      },
    );
  }

  Widget buildDeleteCategory(BuildContext context, bool isAdmin)
  {
    if(isAdmin)
      {
        return Column(
          children: [
            SizedBox(height: 15,),
            TextButton(
              onPressed: ()
              {
                AppCubit.get(context).deleteCategory_categoryDetails(context, AppCubit.get(context).categorty_categoryDetails!.id!);
              },
              child: Container(
                width: double.infinity,
                height: 40,
                color: Colors.blue,
                child: Center(
                  child: Text(
                    "Delete Category",
                    style: TextStyle(
                        color: Colors.white
                    ),
                  ),
                ),
              ),
            )
          ],
        );
      }
    else
      {return Container();}
  }
}
