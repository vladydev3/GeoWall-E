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
recursiveDrawTriangle(p1,p2,p3,n) = 
let
   draw {segment(p1,p2),segment(p2,p3),segment(p3,p1)};
   nextN = n - 1;
   ab = puntoMedio(p1,p2);
   bc = puntoMedio(p2,p3);
   ca = puntoMedio(p3,p1);
   recursion1 = if (nextN > 0) then recursiveDrawTriangle(p1,ab,ca,nextN) else 1;
   recursion2 = if (nextN > 0) then recursiveDrawTriangle(ab,p2,bc,nextN) else 1;
   recursion3 = if (nextN > 0) then recursiveDrawTriangle(ca,bc,p3,nextN) else 1;
in {ab,bc,ca};

point p;
measure m;
a,b,c,_ = regularTriangle(p,m);
draw recursiveDrawTriangle(a,b,c,9);