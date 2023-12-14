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

mediatrix(p1, p2) = 
    let
        l1 = line(p1, p2);
        m1 = measure (p1, p2);
        c1 = circle (p1, m1);
        c2 = circle (p2, m1);
        i1,i2,_ = intersect(c1, c2);
    in line(i1,i2);

//Gets a regular hexagon defined by the circle defined by point p and measure m.
regularHexagon(p,m) =
    let
        point p2;
        l1 = line(p,p2);//    draw l1 "l1";
        c1 = circle(p,m);//   draw c1 "c1";
        i1,i2,_ = intersect(l1,c1);// draw {i1,i2} "intersect(l1,c1)";
        c2 = circle(i1,m);//  draw c2 "c2";
        c3 = circle(i2,m);//  draw c3 "c3";
        i3,i4,_ = intersect(c2,c1);// draw {i3,i4} "intersect(c2,c1)";
        i5,i6,_ = intersect(c3,c1);// draw {i5,i6} "intersect(c3,c1)";
    in {i6,i3,i1,i4,i5,i2};

hexagonalStar(p,m) =
   let 
       v1,v2,v3,v4,v5,v6,_ = regularHexagon(p,m);
       l1 = mediatrix(v1,v2);
       l2 = mediatrix(v2,v3);
       l3 = mediatrix(v3,v4);
       i1,_ = intersect(l1,line(v3,v4));
       i2,_ = intersect(l1,line(v3,v2));
       i3,_ = intersect(l2,line(v1,v2));
       i4,_ = intersect(l2,line(v1,v6));
       i5,_ = intersect(l3,line(v2,v3));
       i6,_ = intersect(l3,line(v2,v1));
   in {segment(v1,i2),segment(i2,v2),segment(v2,i3),segment(i3,v3),segment(v3,i5),segment(i5,v4),segment(v4,i1),segment(i1,v5),segment(v5,i4),segment(i4,v6),segment(v6,i6),segment(i6,v1)};

point p10;
measure m10;
draw hexagonalStar(p10,m10);
