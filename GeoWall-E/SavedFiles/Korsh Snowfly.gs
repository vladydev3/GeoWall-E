mediatrix(p1, p2) = 
    let
        l1 = line(p1, p2);
        m = measure (p1, p2);
        c1 = circle (p1, m);
        c2 = circle (p2, m);
        i1,i2,_ = intersect(c1, c2);
    in line(i1,i2);

drawTriangle(p1,p2,p3) = 
let
   draw {segment(p1,p2),segment(p2,p3),segment(p3,p1)};
in 1;

getTriangleCenter(p1,p2,p3) = 
let
   a = mediatrix(p1,p2);
   b = mediatrix(p2,p3);
   i1,_ = intersect(a,b);
in i1;

regularTriangle(p,m) =
    let
        point p2;
        l1 = line(p,p2);
        c1 = circle(p,m);
        i1,i2,_ = intersect(l1,c1);
        c2 = circle(i1,m);
        c3 = circle(i2,m);
        i3,i4,_ = intersect(c2,c1);
        i5,i6,_ = intersect(c3,c1);
    in {i1,i5,i6};  

getReverseTriangle(p1,p2,p3) =
let
   center = getTriangleCenter(p1,p2,p3);
   c = circle(center,measure(center,p1)+measure(point(0,0),point(0,0.01)));
   i2,_ = intersect(ray(p1,center),c);
   i3,_ = intersect(ray(p2,center),c);
   i1,_ = intersect(ray(p3,center),c);
in {i1,i2,i3};

findSubTriangle(pPivo,p2,p3,pl1,pl2) =
let 
   i1,_ = intersect(line(pPivo,p2),line(pl1,pl2));
   i2,_ = intersect(line(pPivo,p3),line(pl1,pl2));
in {pPivo,i1,i2};

KorshSnowFly(p1,p2,p3,cant) =
if cant > 0 then
   let
    x = drawTriangle(p1,p2,p3);
   
     t1,t2,t3,_ = getReverseTriangle(p1,p2,p3);
   
    t11,t12,t13,_ = findSubTriangle(p1,p2,p3,t1,t3);
    t21,t22,t23,_ = findSubTriangle(t1,t2,t3,p1,p2);
    t31,t32,t33,_ = findSubTriangle(p2,p1,p3,t1,t2);
    t41,t42,t43,_ = findSubTriangle(t2,t3,t1,p2,p3);
    t51,t52,t53,_ = findSubTriangle(p3,p1,p2,t2,t3);
    t61,t62,t63,_ = findSubTriangle(t3,t1,t2,p3,p1);
    color red;
    x1 = KorshSnowFly(t11,t12,t13,cant-1);
    color blue;
    x2 = KorshSnowFly(t21,t22,t23,cant-1);
    color yellow;
    x3 = KorshSnowFly(t31,t32,t33,cant-1);
    color green;
    x4 = KorshSnowFly(t41,t42,t43,cant-1);
    color magenta;
    x5 = KorshSnowFly(t51,t52,t53,cant-1);
    color cyan;
    x6 = KorshSnowFly(t61,t62,t63,cant-1);
   in 1
else 1;

i1,i2,i3,_ = regularTriangle(point(250,250),measure(point(0,0),point(0,150)));
k = KorshSnowFly(i1,i2,i3,4);