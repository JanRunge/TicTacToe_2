-- average epoch time

-- delete dummydata at the bottom
delete from neuron
where network=451 
and neurontime= STR_TO_DATE('2020-02-08 19:36:01.184000','%Y-%c-%d %H:%i:%s.%f')
;



-- The error per epoch
select * from(
select sum(neuronval) summe
from neuron n
where network=448  
group by neurontime) n
where summe>2;

select *
from neuron
where network=451 ;



-- select startingpoint and endpoint of every neuron
select latest_val-first_val diff,mys.* from(
	select (select n.neuronval
				from neuron n
				where n.neurontime=min_time
				and n.idneuron=nnn.idneuron
				and n.network=nnn.network
			) first_val,
			(select n.neuronval
				from neuron n
				where n.neurontime=max_time
				and n.idneuron=nnn.idneuron
				and n.network=nnn.network
			) latest_val
			
			, nnn.idneuron
			,nnn.network
	from (select max(neurontime) max_time
				,min(neurontime) min_time
				,idneuron
				,network 
			from neuron 
			where network=453 
			group by idneuron, network
		) nnn
)mys;


select avg(diff) from(
select latest_val-first_val diff,mys.* from(
	select (select n.neuronval
				from neuron n
				where n.neurontime=min_time
				and n.idneuron=nnn.idneuron
				and n.network=nnn.network
			) first_val,
			(select n.neuronval
				from neuron n
				where n.neurontime=max_time
				and n.idneuron=nnn.idneuron
				and n.network=nnn.network
			) latest_val
			
			, nnn.idneuron
			,nnn.network
	from (select max(neurontime) max_time
				,min(neurontime) min_time
				,idneuron
				,network 
			from neuron 
			where network=451
			group by idneuron, network
		) nnn
)mys)t;


-- 448 -0.00005033325125656148
-- 450 -0.000002870903854273558

select sum(latest_val) from(
select latest_val-first_val diff,mys.* from(
	select (select n.neuronval
				from neuron n
				where n.neurontime=min_time
				and n.idneuron=nnn.idneuron
				and n.network=nnn.network
			) first_val,
			(select n.neuronval
				from neuron n
				where n.neurontime=max_time
				and n.idneuron=nnn.idneuron
				and n.network=nnn.network
			) latest_val
			
			, nnn.idneuron
			,nnn.network
	from (select max(neurontime) max_time
				,min(neurontime) min_time
				,idneuron
				,network 
			from neuron 
			where network=448
			group by idneuron, network
		) nnn
)mys)t;


-- -0.00005033325125656148
-- -0.29117145067471206
-- -0.032616079859204034

-- 1.00000013651557
-- 1.0000001185430878
-- 7.422957896465778
-- 
-- 

select sum(neuronval)
from neuron n
where network=450  
group by neurontime



-- 448-451
select   count(1)
		,network from neuron 
where network>447 
  and idneuron =0 
group by network;
