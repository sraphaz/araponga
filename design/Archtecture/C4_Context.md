C4Context
title Arah — C4 Context (Nível 1)

Person(user, "Usuário", "Morador ou Visitante. Interage com feed, mapa, eventos, alertas e saúde do território.")
Person(moderator, "Curador/Moderador", "Morador verificado com permissões ampliadas para validação e governança.")
Person(sysadmin, "SystemAdmin", "Admin global. Opera configurações do sistema, decide verificações globais e audita ações críticas.")

System(Arah, "Arah", "Plataforma comunitária orientada a território: feed, mapa, entidades e governança mínima.")
System_Ext(identity, "Provedores de Identidade", "Login social (Google, Apple, Microsoft) / OIDC")
System_Ext(payments, "Meios de Pagamento", "Mercado Pago / PayPal (MVP monetização e doações)")
System_Ext(notifications, "Notificações", "Push (FCM/APNs), e-mail (futuro)")
System_Ext(maps, "Mapas e Geocoding", "Tiles, geocoding/reverse geocoding (futuro)")

Rel(user, Arah, "Usa para explorar e contribuir com o território", "HTTPS")
Rel(moderator, Arah, "Modera e valida entidades do território", "HTTPS")
Rel(sysadmin, Arah, "Administra configurações e decide filas globais", "HTTPS")

Rel(Arah, identity, "Autentica usuários", "OIDC/OAuth2")
Rel(Arah, payments, "Processa pagamentos/doações", "HTTPS")
Rel(Arah, notifications, "Envia notificações", "HTTPS")
Rel(Arah, maps, "Consulta serviços de mapas", "HTTPS")
