--DROP TABLE "members";

CREATE TABLE "members"
(
  "id" serial NOT NULL,
  "username" character varying(50),
  "fullname" character varying(100),
  "isactive" BOOLEAN,
   UNIQUE ("username"),
  CONSTRAINT "members_pkey" PRIMARY KEY ("id")	
)
WITH (
  OIDS=FALSE
);

ALTER TABLE "members"
  OWNER TO postgres;

COMMENT ON TABLE "members"
  IS 'The table of members';

--
--DROP TABLE "teams";

CREATE TABLE "teams"
(
  "id" serial NOT NULL,
  "name" CHARACTER VARYING(50),
  "projectkey" CHARACTER VARYING(50),
  "isactive" BOOLEAN,
   UNIQUE ("name","projectkey"),
  CONSTRAINT "teams_pkey" PRIMARY KEY ("id")	
)
WITH (
  OIDS=FALSE
);

ALTER TABLE "teams"
  OWNER TO postgres;

COMMENT ON TABLE "teams"
  IS 'The table of teams';
	
--
--DROP TABLE "team_member"

CREATE TABLE "team_member"
(
 -- "id" serial NOT NULL,
 --	"team_id" integer,
 -- "member_id" integer,
 --CONSTRAINT "tm_pkey" PRIMARY KEY ("id"),
	"team_id" integer NOT NULL,
  "member_id" integer NOT NULL,
  UNIQUE ("team_id","member_id"),
  CONSTRAINT "tm_pkey" PRIMARY KEY("team_id","member_id"),
	
	CONSTRAINT "tm_team_id_fkey" FOREIGN KEY ("team_id")
      REFERENCES "teams" ("id") MATCH SIMPLE
      ON UPDATE CASCADE ON DELETE CASCADE,
			
	CONSTRAINT "tm_member_id_fkey" FOREIGN KEY ("member_id")
	REFERENCES "members" ("id") MATCH SIMPLE
	ON UPDATE CASCADE ON DELETE CASCADE
)
WITH (
  OIDS=FALSE
);

ALTER TABLE "team_member"
  OWNER TO postgres;

COMMENT ON TABLE "team_member"
  IS 'The table of team_member';

