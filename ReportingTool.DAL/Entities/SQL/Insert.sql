INSERT INTO "members" ("username", "fullname", "isactive") VALUES (N'username1', N'fullname1', TRUE);
INSERT INTO "members" ("username", "fullname", "isactive") VALUES (N'username2', N'fullname2', TRUE);
INSERT INTO "members" ("username", "fullname", "isactive") VALUES (N'username3', N'fullname3', TRUE);
INSERT INTO "members" ("username", "fullname", "isactive") VALUES (N'username4', N'fullname4', TRUE);
INSERT INTO "members" ("username", "fullname", "isactive") VALUES (N'username5', N'fullname5', TRUE);
--
INSERT INTO "teams" ("name",  "projectkey", "isactive") VALUES (N'name1', N'projectkey1', TRUE);
INSERT INTO "teams" ("name", "projectkey", "isactive") VALUES (N'name2', N'projectkey1', TRUE);
INSERT INTO "teams" ("name", "projectkey", "isactive") VALUES (N'name3', N'projectkey2', TRUE);
INSERT INTO "teams" ("name", "projectkey", "isactive") VALUES (N'name4', N'projectkey2', TRUE);
INSERT INTO "teams" ("name", "projectkey", "isactive") VALUES (N'name5', N'projectkey2', TRUE);
--
INSERT INTO "team_member" ("team_id", "member_id") VALUES (1, 1);
INSERT INTO "team_member" ("team_id", "member_id") VALUES (1, 2);
INSERT INTO "team_member" ("team_id", "member_id") VALUES (2, 3);

INSERT INTO "team_member" ("team_id", "member_id") VALUES (2, 4);
INSERT INTO "team_member" ("team_id", "member_id") VALUES (3, 1);

INSERT INTO "team_member" ("team_id", "member_id") VALUES (3, 2);
INSERT INTO "team_member" ("team_id", "member_id") VALUES (3, 4);

INSERT INTO "team_member" ("team_id", "member_id") VALUES (4, 3);
INSERT INTO "team_member" ("team_id", "member_id") VALUES (4, 5);

INSERT INTO "team_member" ("team_id", "member_id") VALUES (5, 1);

--



