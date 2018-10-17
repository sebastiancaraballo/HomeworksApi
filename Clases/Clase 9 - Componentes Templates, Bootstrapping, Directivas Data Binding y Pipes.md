# Componentes, Templates, Bootstrapping, Directivas, Data Binding y Pipes

## Repasamos el concepto de Componente

### Fundamentos de los componentes

Cada componente la idea es que funcione armoniosamente y en conjunto con el resto para proveer una experiencia de usuario única. Como dijimos, estos son modulares, resuelven un problema concreto y colaboran entre sí para lograr ir armando la interfaz de usuario como un puzzle donde cada pieza tiene sus diferentes responsabilidades.

Por ejemplo, una excelente forma de pensar los componentes es a través de la siguiente imagen:

![imagen](imgs/angular-clase2/angular-components-sample-2.png)

A su vez, es interesante recordar cómo se comporta internamente cada componente. Como habíamos dicho, los componentes se componen de tres cosas:

1) **Template:** El **template** del componente, el cual define la estructura (HTML o la vista) del mismo. Se crea con código html y define lo que se renderizará en a página. A su vez incluye *bindings* y *directivas* para darle poder a nuestra vista y hacerla dinámica. Estos dos conceptos los veremos más adelante en este módulo

2) **Clase:** A su vez la view tiene un código asociado, el cual llamamos la **clase** de un componente. Esta representa el código asociado a la vista (creada con TypeScript), la cual posee los *datos*, llamadas las *properties* para que la vista los use (el nombre de una mascota por ejemplo), y a su vez posee la *lógica/funciones* que usan los mismos. Por ejemplo: la lógica para mostrar o esconder una imagen, la lógica para traer datos desde una Api, etc.

3) **Metadata:** Finalmente, el componente también tiene **metadata**, que es información adicional para Angular, siendo esta definida con un *decorator* (los que arrancan con **@**). Un decorador es una función que agrega metadata a una clase, sus miembros o los argumentos de sus métodos.

![imagen](imgs/angular-clase2/angular_component_architecture.png)

### La clase de un componente:

Para la clase de un componente, la sintaxis es como sigue a continuación:

```typescript
export class NombreComponent {
  property1: tipo = valor,
  property2: tipo2 = valor2,
  property3: tipo2 = valor3
  ...
}
```

Pero a su vez necesitamos utilizar el decorador ```Component```, el cual lo debemos importar desde ```@angular/core```:

```typescript
import { Component } from '@angular/core';

@Component({
  selector: 'nombre-component',
  template: `<h1>Hello {{property1}}</h1>`
})
export class NombreComponent {
  property1: tipo = valor,
  property2: tipo2 = valor2,
  property3: tipo2 = valor3
  ...
}
```

También podemos definir un constructor:

```typescript

export class NombreComponent {
  property1: string;
  property2: number;

  constructor(property1:string, property2: number) {
    this.property1 = property1
    this.property2 = property2;
  }
}

```

Algunos otros conceptos a tener en cuenta:

- Recordemos que por convención, el componente fundamental de una app de angular se llama AppComponent (el root component).

- La palabra reservada “export” simplemente hace que el componente se exporte y pueda ser visto por otros componentes de nuestra app.

- La sintaxis de definición del archivo es ```nombre.component.ts```.

- El valor por defecto en las properties de nuestros componentes es opcional.

- Los métodos vienen luego de las properties, en lowerCamelCase.
