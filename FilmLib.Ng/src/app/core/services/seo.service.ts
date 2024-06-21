import { Injectable } from '@angular/core';
import { Meta } from '@angular/platform-browser';
@Injectable({
  providedIn: 'root',
})
export class SeoService {
  constructor(private meta: Meta) {}
  generateTags(config: any) {
    // default values
    config = {
      title: 'Angular Movies',
      description: 'My SEO friendly Angular Component',
      slug: '',
      ...config,
    };
    this.meta.updateTag({ property: 'og:type', content: 'article' });
    this.meta.updateTag({ property: 'og:site_name', content: 'AngularMovie' });
    this.meta.updateTag({ property: 'og:title', content: config.title });
    this.meta.updateTag({
      property: 'og:description',
      content: config.description,
    });
  }
}
