INSERT INTO "members" ("username", "fullname") VALUES (N'username1', N'fullname1');
INSERT INTO "members" ("username", "fullname") VALUES (N'username2', N'fullname2');
INSERT INTO "members" ("username", "fullname") VALUES (N'username3', N'fullname3');
INSERT INTO "members" ("username", "fullname") VALUES (N'username4', N'fullname4');
INSERT INTO "members" ("username", "fullname") VALUES (N'username5', N'fullname5');
--
INSERT INTO "teams" ("name", "isactive") VALUES (N'name1', TRUE);
INSERT INTO "teams" ("name", "isactive") VALUES (N'name2', TRUE);
INSERT INTO "teams" ("name", "isactive") VALUES (N'name3', TRUE);
INSERT INTO "teams" ("name", "isactive") VALUES (N'name4', FALSE);
INSERT INTO "teams" ("name", "isactive") VALUES (N'name5', FALSE);
--
INSERT INTO "teams_member" ("team_id", "member_id") VALUES (1, 1);
INSERT INTO "teams_member" ("team_id", "member_id") VALUES (1, 2);
INSERT INTO "teams_member" ("team_id", "member_id") VALUES (2, 3);
INSERT INTO "teams_member" ("team_id", "member_id") VALUES (2, 4);
INSERT INTO "teams_member" ("team_id", "member_id") VALUES (3, 1);
INSERT INTO "teams_member" ("team_id", "member_id") VALUES (3, 1);
INSERT INTO "teams_member" ("team_id", "member_id") VALUES (4, 3);
INSERT INTO "teams_member" ("team_id", "member_id") VALUES (4, 5);
INSERT INTO "teams_member" ("team_id", "member_id") VALUES (5, 1);

--



"id" serial NOT NULL,
	"team_id" integer,
  "member_id" integer,
