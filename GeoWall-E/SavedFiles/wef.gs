﻿//Gets a regular hexagon defined by the circle defined by point p and measure m.
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
    in {i1,i3,i5,i2,i6,i4};

//Gets the mediatrix of the line defined by p1 and p2.
mediatrix(p1, p2) = 
    let
        l1 = line(p1, p2);
        m = measure (p1, p2);
        c1 = circle (p1, m);
        c2 = circle (p2, m);
        i1,i2,_ = intersect(c1, c2);
    in line(i1,i2);

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
   in {v1,i2,v2,i3,v3,i5,v4,i1,v5,i4,v6,i6};

getSpikes(p1,p2,p3,m) =
      if m / measure(p2,p3) > 35 then {} 
      else let
              l1 = mediatrix(p1,p2);
              l2 = mediatrix(p1,p3);
              i1,_ = intersect(l1,line(p1,p2));
              i2,_ = intersect(l2,line(p1,p3));
              i3,_ = intersect(l1,l2);
              draw {segment(i1,i3), segment(i2,i3),segment(i3,p1)};
              in {i1,i2,i3} + getSpikes(i1,p2,i3,m) + getSpikes(i2,p3,i3,m);
        


drawRecursiveSnowFly(p,m) = 
   let
      p1,p2,p3,p4,p5,p6,p7,p8,p9,p10,p11,p12,_ = hexagonalStar(p,m);
      m1 = measure(p1,p2);
      s1 = getSpikes(p1,p2,p12,m);
      s2 = getSpikes(p3,p2,p4,m);
      s3 = getSpikes(p5,p4,p6,m);
      s4 = getSpikes(p7,p6,p8,m);
      s5 = getSpikes(p9,p8,p10,m);
      s6 = getSpikes(p11,p10,p12,m);
      draw 
      {
        segment(p1,p2),segment(p2,p3),segment(p3,p4),segment(p4,p5),
        segment(p5,p6),segment(p6,p7),segment(p7,p8),segment(p8,p9),
        segment(p9,p10),segment(p10,p11),segment(p11,p12),segment(p12,p1),
        segment(p1,p),segment(p2,p),segment(p3,p),segment(p4,p),segment(p5,p),
        segment(p6,p),segment(p7,p),segment(p8,p),segment(p9,p),segment(p10,p),
        segment(p11,p),segment(p12,p)
      };
   in 0;
   
   
   
   
pu1 = point(150,0);
pu2 = point(0,0);
m = measure(pu1,pu2);


a = drawRecursiveSnowFly(point(450,300),m);
