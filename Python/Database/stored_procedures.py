from sqlalchemy.sql import text

def create_stored_procedures(engine):
    mw_upsert_user = """create or replace procedure mw_upsert_user(user_name varchar(255),
	                                  	   password varchar(255))
                                language 'plpgsql'
                                as $$
                                begin
	                                insert into base_db.public."user" values (default, user_name, password)
									on conflict ("name")
									do
									   update set "passHash" = password where user_name = user_name;
                                end; $$"""

    gt_user_by_name = """create or replace function gt_user_by_name(user_name varchar(255))
                      returns table (userid bigint,
                                   	 name varchar(255),
                                 	 password varchar(255)
                                  	) as $$
                      begin
                            return query
                            select
                            u."user_id",
                            u.name,
                            u."passHash"
                            from "user" as u
                            where u."name" = user_name;
                      end; $$
                      language 'plpgsql'; """

    mw_update_user_location = """create or replace procedure mw_update_user_location(id bigint,
				                                  	geohash varchar(255))
                                language 'plpgsql'
                                as $$
                                begin
	                                insert into base_db.public.user_location values (id, geohash)
									on conflict  (user_id)
									do
									   update set geo_hash = geohash where user_location.user_id = id;
                                end; $$"""

    gt_users_in_geohash = """create or replace function 	gt_users_in_geohash(gridhash varchar)
                                  returns table (user_id bigint,
			                                   	 name varchar(255)
			                                  	) as $$
                                  begin
                                        return query
                                        select
                                        ul.user_id,
                                        u.name
                                        from user_location as ul
                                        join base_db.public."user" as u on u.user_id = ul.user_id
                                        where ul.geo_hash = gridhash;
                                  end; $$
                                  language 'plpgsql';"""

    mw_create_interaction = """create or replace procedure mw_create_interaction(primaryUserId bigint,
												  interactedWithUserId bigint,
			                                  	  geohash varchar(255))
                                language 'plpgsql'
                                as $$
                                begin
	                                insert into base_db.public.interaction values (default, now(), primaryUserId, interactedWithUserId, geohash);
                                end; $$"""

    gt_user_interactions = """create or replace function 	gt_user_interactions(user_id bigint)
                                  returns table (interaction_id bigint,
                                  				 interaction_time timestamp,
                                  				 primary_user_id bigint,
			                                   	 interacted_with_user_id bigint,
			                                   	 interacted_at_hash varchar(255)
			                                  	) as $$
                                  begin
                                        return query
                                        select
                                        i.interaction_id,
                                        i.interaction_time,
                                        i.primary_user_id,
                                        i.interacted_with_user_id,
                                        i.interacted_at_hash
                                        from interaction as i
                                        where i.primary_user_id = user_id;
                                  end; $$
                                  language 'plpgsql';"""

    mw_create_friend = """create or replace procedure mw_create_friend(primaryUserId bigint,
											  friendUserId bigint,
			                              	  interactionId bigint)
                language 'plpgsql'
            as $$
            begin
                insert into base_db.public.friend values (primaryUserId, friendUserId, interactionId);
            end; $$"""

    gt_friends = """create or replace function 	gt_friends(user_id bigint)
                                  returns table (primaryUserId bigint,
											     friendUserId bigint,
			                              	     interactionId bigint
			                                  	) as $$
                                  begin
                                        return query
                                        select
                                        f.primary_user_id,
                                        f.friend_user_id,
                                        f.interaction_id
                                        from friend as f
                                        where f.primary_user_id = user_id;
                                  end; $$
                                  language 'plpgsql';"""

    mw_create_landmark = """create or replace procedure mw_create_landmark(geohash varchar(255),
			                              	    name varchar(255))
                language 'plpgsql'
            as $$
            begin
                insert into base_db.public.landmark values (default, geohash, name);
            end; $$"""

    gt_landmarks = """create or replace function 	gt_landmarks()
                                  returns table (landmark_id bigint,
											     geo_hash varchar(255),
			                              	     display_name varchar(255)
			                                  	) as $$
                                  begin
                                        return query
                                        select
                                        l.landmark_id,
                                        l.geo_hash,
                                        l.display_name
                                        from landmark as l;
                                  end; $$
                                  language 'plpgsql';"""

    with engine.connect() as con:
        con.execute(mw_upsert_user)
        print("created procedure mw_upsert_user")

        con.execute(gt_user_by_name)
        print("created procedure gt_user_by_name")

        con.execute(mw_update_user_location)
        print("created procedure mw_update_user_location")

        con.execute(gt_users_in_geohash)
        print("created procedure gt_users_in_geohash")

        con.execute(mw_create_interaction)
        print("created procedure mw_create_interaction")

        con.execute(gt_user_interactions)
        print("created procedure gt_user_interactions")

        con.execute(mw_create_friend)
        print("created procedure mw_create_friend")

        con.execute(gt_friends)
        print("created procedure gt_friends")

        con.execute(mw_create_landmark)
        print("created procedure mw_create_landmark")

        con.execute(gt_landmarks)
        print("created procedure gt_landmarks")
