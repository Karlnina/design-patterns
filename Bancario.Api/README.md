# Guia Implementacion: Patrones de Diseno en una API Bancaria (.NET 9)

## Objetivo
Este documento explica como se implementan y colaboran los patrones de diseno en el proyecto unico de API bancaria. El foco es didactico: mostrar el problema, la solucion y la interaccion entre patrones en un flujo real.

## Proyecto
- API: Minimal APIs con .NET 9
- Archivo central de implementacion: PatronesBancariosApi/BankingModule.cs
- Flujo principal: POST /api/transactions/process
- Flujo por lotes: POST /api/transactions/batch/process
- Diagnostico de patrones: Header X-Debug-Patterns: true

## Flujo end-to-end de una transaccion
1. Se recibe la solicitud de transferencia.
2. Builder crea el agregado BankTransaction.
3. Template Method orquesta pasos fijos del proceso.
4. Composite valida reglas de negocio.
5. Proxy controla acceso a cuentas y fondos.
6. Factory Method elige procesador domestico o internacional.
7. Strategy decide la ruta de ejecucion segun prioridad.
8. Decorator compone costos adicionales de forma dinamica.
9. Singleton convierte el monto final a USD con tasas compartidas.
10. Adapter integra gateway legacy o moderno.
11. Abstract Factory genera documentos regionales.
12. Observer emite notificaciones post-proceso.
13. Command encapsula la solicitud como operacion ejecutable.
14. Iterator procesa lotes de transacciones sin exponer estructura interna.
15. DI conecta todas las piezas sin acoplamiento fuerte.

## Patron por patron: como y para que

### 1) Factory Method
- Como: ProcessorFactory selecciona un creator y obtiene ITransactionProcessor concreto.
- Para que: encapsular la decision de que procesador usar sin ifs regados por todo el codigo.
- Colaboracion: usa datos construidos por Builder y se ejecuta dentro de Template Method.

### 2) Builder
- Como: TransactionBuilder transforma TransactionRequest en BankTransaction completo y consistente.
- Para que: centralizar la construccion de objetos complejos y evitar inicializaciones incompletas.
- Colaboracion: es el primer paso en Orchestrator y alimenta todo el pipeline.

### 3) Singleton
- Como: ExchangeRateRegistry usa instancia unica con Lazy.
- Para que: compartir una misma fuente de tasas de conversion en toda la app.
- Colaboracion: se invoca despues del calculo de fees para normalizar el monto final.

### 4) Abstract Factory
- Como: DocumentFactoryResolver elige una fabrica regional que crea recibo y auditoria compatibles.
- Para que: garantizar consistencia entre familias de documentos por region.
- Colaboracion: se activa cuando la transaccion ya fue ejecutada y necesita evidencia documental.

### 5) Proxy
- Como: AccountServiceProxy envuelve IAccountService real y aplica control de acceso/fondos.
- Para que: agregar seguridad y reglas previas sin tocar la logica base de cuentas.
- Colaboracion: se usa en la fase de validacion del Template Method.

### 6) Adapter
- Como: LegacyCoreBankingAdapter y ModernBankingApiAdapter exponen un contrato unico IPaymentAdapter.
- Para que: integrar sistemas externos heterogeneos con una interfaz uniforme.
- Colaboracion: el selector de adapter se alimenta del request y se ejecuta en el paso de cobro.

### 7) Decorator
- Como: cadena BaseFee -> TaxFee -> InsuranceFee -> PriorityFee.
- Para que: componer cargos sin crear subclases para cada combinacion posible.
- Colaboracion: trabaja junto con Strategy para obtener el costo final.

### 8) Composite
- Como: ValidationComposite recorre reglas IValidationRule y corta en el primer error.
- Para que: tratar reglas individuales y conjunto de reglas de la misma forma.
- Colaboracion: se ejecuta antes de cualquier cargo para cortar fallos temprano.

### 9) Iterator
- Como: TransactionBatch implementa IEnumerable y controla el recorrido interno.
- Para que: procesar lotes de transacciones con un contrato de iteracion estable.
- Colaboracion: cada item del lote se ejecuta como Command independiente.

### 10) Command
- Como: ProcessTransactionCommand encapsula la accion de procesar una transaccion.
- Para que: desacoplar invocacion de ejecucion y habilitar extensiones (cola, reintentos, auditoria).
- Colaboracion: CommandBus ejecuta comandos, y Orchestrator realiza el trabajo real.

### 11) Strategy
- Como: RouteStrategyResolver selecciona FastRouteStrategy o EconomicRouteStrategy.
- Para que: intercambiar algoritmos de enrutamiento sin tocar el flujo general.
- Colaboracion: su salida impacta el calculo de fees via Decorator.

### 12) Observer
- Como: TransactionSubject notifica a EmailObserver y AuditObserver al completar.
- Para que: reaccionar a eventos sin acoplar emisores con receptores concretos.
- Colaboracion: se ejecuta al final del Template Method como side-effect controlado.

### 13) Template Method
- Como: TransferTransactionTemplate define la secuencia fija Validate -> Prepare -> Charge -> GenerateDocuments -> Notify.
- Para que: mantener el flujo del negocio estable y extensible por pasos.
- Colaboracion: es el pegamento principal entre todos los patrones operativos.

### 14) Dependency Injection
- Como: AddBankingModule registra colaboradores por ciclo de vida apropiado.
- Para que: lograr bajo acoplamiento y alta testabilidad.
- Colaboracion: hace posible intercambiar implementaciones de Strategy, Adapter, Observer, etc.

## Matriz rapida de colaboracion
- Builder -> Template Method
- Composite + Proxy -> Validate
- Factory Method + Strategy + Decorator + Singleton -> Prepare
- Adapter + Proxy -> Charge
- Abstract Factory -> GenerateDocuments
- Observer -> Notify
- Command -> dispara Orchestrator
- Iterator -> multiplica ejecucion de Command en batch
- DI -> conecta todo el grafo de dependencias

## Endpoints principales
- POST /api/transactions/process
- POST /api/transactions/batch/process
- GET /api/patterns

## Ejemplo minimo de request
```json
{
  "fromAccount": "ACC-001",
  "toAccount": "ACC-002",
  "amount": 1200,
  "currency": "USD",
  "region": "LATAM",
  "gateway": "legacy",
  "priority": "fast",
  "includeInsurance": true,
  "isInternational": false,
  "requestedBy": "arquitecto.demo"
}
```

## Notas didacticas
- La API prioriza claridad arquitectonica sobre persistencia avanzada.
- Los patrones no se muestran aislados: colaboran dentro de un mismo caso de uso.
- Si se envia X-Debug-Patterns: true, la respuesta incluye trazabilidad del pipeline.
- Disponible en swagger: http://localhost:5165/swagger/index.html
