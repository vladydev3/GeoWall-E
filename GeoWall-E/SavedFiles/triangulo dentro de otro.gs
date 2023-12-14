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

mediatriz(p1, p2) = 
    let
        l1 = line(p1, p2);
        m1 = measure (p1, p2);
        c1 = circle (p1, m1);
        c2 = circle (p2, m1);
        i1,i2,_ = intersect(c1, c2);
    in line(i1,i2);
puntoMedio(p1,p2)=
    let
       medio,_ = intersect(line(p1,p2), mediatriz(p1,p2));
    in medio;

drawTriangle(p1,p2,p3) = 
let
   draw {segment(p1,p2),segment(p2,p3),segment(p3,p1)};
in 1;

point p;
measure m;
a,b,c,_ = regularTriangle(p,m);
e = drawTriangle(a,b,c);
ab = puntoMedio(a,b);
bc = puntoMedio(b,c);
ca = puntoMedio(c,a);
d = drawTriangle(ab,bc,ca);
