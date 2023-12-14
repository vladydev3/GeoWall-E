mediatrix(p1, p2) = 
    let
        l1 = line(p1, p2);
        m1 = measure (p1, p2);
        c1 = circle (p1, m1);
        c2 = circle (p2, m1);
        i1,i2,_ = intersect(c1, c2);
    in line(i1,i2);

puntoMedio(p1,p2)=
    let
       medio,_ = intersect(line(p1,p2), mediatrix(p1,p2));
    in medio;

point p10;
point p20;
draw line(p10,p20);
draw puntoMedio(p10,p20) "punto medio";