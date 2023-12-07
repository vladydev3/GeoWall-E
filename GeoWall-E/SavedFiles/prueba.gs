import "Mediatriz.gs";

p1 = point(250,250);	draw p1 "p1";
p2 = point(250,300);	draw p2 "p2";
p3 = point(300,250);	draw p3 "p3";

med = mediatriz(p2,p3);	draw med;

m = measure(p1,p2);

a = arc(p1,p3,p2,m);

draw a "a";

color red;
draw intersect(a,med);