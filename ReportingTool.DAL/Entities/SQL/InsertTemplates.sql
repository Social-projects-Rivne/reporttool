INSERT INTO public."Templates"("Id", "Name", "IsActive", "Owner") VALUES (1, 'Template1', true, 'ssund');
INSERT INTO public."Templates"("Id", "Name", "IsActive", "Owner") VALUES (2, 'Template2', true, 'ssund');
INSERT INTO public."Templates"("Id", "Name", "IsActive", "Owner") VALUES (3, 'Template3', false, 'ssundtc');
INSERT INTO public."Templates"("Id", "Name", "IsActive", "Owner") VALUES (4, 'Template4', false, 'ssundtc');
INSERT INTO public."Templates"("Id", "Name", "IsActive", "Owner") VALUES (5, 'Template5', true, 'ssundtc');

INSERT INTO public."FieldsInTemplates"("Id", "DefaultValue", "FieldId", "TemplateId") VALUES (1, 'reporter1', 1, 1);
INSERT INTO public."FieldsInTemplates"("Id", "DefaultValue", "FieldId", "TemplateId") VALUES (2, 'receiver1', 2, 1);
INSERT INTO public."FieldsInTemplates"("Id", "DefaultValue", "FieldId", "TemplateId") VALUES (3, 'task1', 3, 1);
INSERT INTO public."FieldsInTemplates"("Id", "DefaultValue", "FieldId", "TemplateId") VALUES (4, 'reporter2', 1, 2);
INSERT INTO public."FieldsInTemplates"("Id", "DefaultValue", "FieldId", "TemplateId") VALUES (5, 'receiver1', 2, 2);
INSERT INTO public."FieldsInTemplates"("Id", "DefaultValue", "FieldId", "TemplateId") VALUES (6, 'task2', 3, 5);
INSERT INTO public."FieldsInTemplates"("Id", "DefaultValue", "FieldId", "TemplateId") VALUES (7, 'reporter1', 1, 5);

