mediatriz(p1, p2) = 
    let
		l1 = line(p1, p2);
		m = measure (p1, p2);
		c1 = circle (p1, m);
		c2 = circle (p2, m);
		i1,i2,_ = intersect(c1, c2);
		l2 = line(i1, i2);
    in l2;

puntoMedio(p1,p2)=
    let
       medio,_ = intersect(line(p1,p2), mediatriz(p1,p2));
    in medio;


point p1;
point p2;

draw puntoMedio(p1, p2) "Medio";

