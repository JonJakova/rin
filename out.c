#include <stdio.h>
int main(void){
float a;
float b;
printf("Enter two numbers!\n");
if(0 == scanf("%f", &a)) {
a =0;
scanf("%*s");
}
if(0 == scanf("%f", &b)) {
b =0;
scanf("%*s");
}
if(a>b){
printf("%.2f\n", (float)(a));
printf("is bigger than \n");
printf("%.2f\n", (float)(b));
}
if(a<b){
printf("%.2f\n", (float)(b));
printf("is bigger than \n");
printf("%.2f\n", (float)(a));
}
return 0;
}
