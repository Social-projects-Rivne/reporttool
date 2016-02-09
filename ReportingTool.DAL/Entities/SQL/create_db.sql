CREATE TABLE "templates"
(
  "id" serial NOT NULL,
  "name" character varying(50),
  "owner" character varying(100),
  "is_active" BOOLEAN NOT NULL,
   UNIQUE ("name"),
  CONSTRAINT "templates_pkey" PRIMARY KEY ("id")	
)
WITH (
  OIDS=FALSE
);

ALTER TABLE "templates"
  OWNER TO postgres;

COMMENT ON TABLE "templates"
  IS 'The table of templates';


CREATE TABLE "fields"
(
  "id" serial NOT NULL,
  "name" CHARACTER VARYING(50),
  UNIQUE ("name"),
  CONSTRAINT "fields_pkey" PRIMARY KEY ("id")	
)
WITH (
  OIDS=FALSE
);

ALTER TABLE "fields"
  OWNER TO postgres;

COMMENT ON TABLE "fields"
  IS 'The table of fields';
	

CREATE TABLE "field_in_template"
(
 "template_id" integer NOT NULL,
  "field_id" integer NOT NULL,
  "default" CHARACTER VARYING(100),
   CONSTRAINT "tf_pkey" PRIMARY KEY("template_id","field_id"),
	
	CONSTRAINT "tf_template_id_fkey" FOREIGN KEY ("template_id")
      REFERENCES "templates" ("id") MATCH SIMPLE
      ON UPDATE CASCADE ON DELETE CASCADE,
			
	CONSTRAINT "tf_field_id_fkey" FOREIGN KEY ("field_id")
	REFERENCES "fields" ("id") MATCH SIMPLE
	ON UPDATE CASCADE ON DELETE CASCADE
)
WITH (
  OIDS=FALSE
);

ALTER TABLE "field_in_template"
  OWNER TO postgres;

COMMENT ON TABLE "field_in_template"
  IS 'The table of field_in_template';